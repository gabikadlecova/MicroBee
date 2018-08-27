using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MicroBee.Data
{
	class HttpService
	{
		private readonly HttpClient _client;
		private readonly StringBuilder _builder = new StringBuilder();

		private readonly TimeSpan _timeEpsilon = new TimeSpan(0, 0, 0, 0, 100);
		private string LoginPath { get; }
		private string RegisterPath { get; }

		private bool Authenticated { get; set; }

		public HttpService(string host, string loginPath, string registerPath)
		{
			_client = new HttpClient();
			_client.BaseAddress = new Uri(host);

			LoginPath = loginPath;
			RegisterPath = registerPath;

			Authenticated = false;
		}

		public async Task LoginAsync(string username, string password)
		{
			Logout();

			var model = new
			{
				Username = username,
				Password = password
			};

			string json = JsonConvert.SerializeObject(model);
			HttpContent content = new StringContent(json);
			content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = await _client.PostAsync(LoginPath, content);
			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
			}

			JwtToken token = JsonConvert.DeserializeObject<JwtToken>(response.Content.ToString());
			await SetCredentialsAsync(username, password, token);

			Authenticated = true;
		}

		public void Logout()
		{
			SecureStorage.Remove("username");
			SecureStorage.Remove("password");
			SecureStorage.Remove("token");
			SecureStorage.Remove("token_expire");

			Authenticated = false;
		}

		public async Task RegisterAsync(string username, string email, string password)
		{
			Logout();

			var model = new
			{
				Username = username,
				Password = password,
				Email = email
			};

			string json = JsonConvert.SerializeObject(model);
			HttpContent content = new StringContent(json);
			content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = await _client.PostAsync(RegisterPath, content);
			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
			}

			JwtToken token = JsonConvert.DeserializeObject<JwtToken>(response.Content.ToString());
			await SetCredentialsAsync(username, password, token);

			Authenticated = true;
		}

		public async Task<T> GetAsync<T>(string path, List<KeyValuePair<string, object>> parameters, bool authorize = false)
		{
			if (authorize)
			{
				await CheckTokenLifetimeAsync();
			}

			Uri uri = new Uri(CreateUri(path, parameters));

			var response = await _client.GetAsync(uri);
			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
			}

			return JsonConvert.DeserializeObject<T>(response.Content.ToString());
		}

		public async Task PostAsync<T>(string path, List<KeyValuePair<string, object>> parameters, T item, bool authorize = false)
		{
			if (authorize)
			{
				await CheckTokenLifetimeAsync();
			}

			Uri uri = new Uri(CreateUri(path, parameters));

			string json = JsonConvert.SerializeObject(item);
			HttpContent content = new StringContent(json);
			content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = await _client.PostAsync(uri, content);

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
			}
		}

		public async Task PutAsync<T>(string path, List<KeyValuePair<string, object>> parameters, T item, bool authorize = false)
		{
			if (authorize)
			{
				await CheckTokenLifetimeAsync();
			}

			Uri uri = new Uri(CreateUri(path, parameters));

			string json = JsonConvert.SerializeObject(item);
			HttpContent content = new StringContent(json);
			content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = await _client.PutAsync(uri, content);

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
			}
		}

		public async Task DeleteAsync<TKey>(string path, List<KeyValuePair<string, object>> parameters, TKey id, bool authorize = false)
		{
			if (authorize)
			{
				await CheckTokenLifetimeAsync();
			}

			Uri uri = new Uri(CreateUri(path + id, parameters));

			var response = await _client.DeleteAsync(uri);

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
			}
		}

		private async Task CheckTokenLifetimeAsync()
		{
			if (!Authenticated)
			{
				throw new NotAuthenticatedException();
			}

			var dateString = await SecureStorage.GetAsync("token_expire");
			DateTime expireDateTime = DateTime.Parse(dateString, CultureInfo.InvariantCulture);
			
			if (DateTime.Now + _timeEpsilon > expireDateTime)
			{
				string username = await SecureStorage.GetAsync("username");
				string password = await SecureStorage.GetAsync("password");

				await LoginAsync(username, password);
			}
		}

		private async Task SetCredentialsAsync(string username, string password, JwtToken token)
		{
			await SecureStorage.SetAsync("username", username);
			await SecureStorage.SetAsync("password", password);
			await SecureStorage.SetAsync("token", token.TokenString);
			await SecureStorage.SetAsync("token_expire", token.ExpireDateTime.ToString(CultureInfo.InvariantCulture));
		}

		private string CreateUri(string path, List<KeyValuePair<string, object>> parameters)
		{
			if (parameters == null)
			{
				return path;
			}

			_builder.Clear();
			_builder.Append(path);
			_builder.Append('?');

			foreach (var parameter in parameters)
			{
				_builder.Append(parameter.Key);
				_builder.Append('=');
				_builder.Append(parameter.Value);
				_builder.Append('&');
			}

			// remove last '&'
			_builder.Remove(_builder.Length - 2, 1);

			return _builder.ToString();
		}

		private class JwtToken
		{
			public string TokenString { get; set; }
			public DateTime ExpireDateTime { get; set; }
		}
	}
}
