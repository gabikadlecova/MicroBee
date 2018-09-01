using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MicroBee.Data.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MicroBee.Data
{
	public class HttpService
	{
		private readonly HttpClient _client;
		private readonly StringBuilder _builder = new StringBuilder();

		private readonly TimeSpan _timeEpsilon = new TimeSpan(0, 0, 0, 0, 100);
		private string LoginPath { get; }
		private string RegisterPath { get; }

		public bool Authenticated { get; private set; }
		public string Username { get; private set; }


		public HttpService(string host, string loginPath, string registerPath)
		{
			_client = new HttpClient();
			_client.BaseAddress = new Uri(host);

			LoginPath = loginPath;
			RegisterPath = registerPath;

			Authenticated = false;
		}

		public async Task LoginAsync(LoginModel model)
		{
			Logout();

			JwtToken token = await PostAsync<LoginModel, JwtToken>(LoginPath, model);
			await SetCredentialsAsync(model.Username, model.Password, token);

			Authenticated = true;
		}

		public void Logout()
		{
			Authenticated = false;

			SecureStorage.Remove("username");
			Username = null;
			SecureStorage.Remove("password");
			SecureStorage.Remove("token");
			SecureStorage.Remove("token_expire");

			_client.DefaultRequestHeaders.Authorization = null;
		}

		public async Task RegisterAsync(RegisterModel model)
		{
			Logout();

			JwtToken token = await PostAsync<RegisterModel, JwtToken>(RegisterPath, model);
			await SetCredentialsAsync(model.Username, model.Password, token);

			Authenticated = true;
		}

		public async Task<T> GetAsync<T>(string path, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			var content = await GetAsyncInternal(path, parameters, authorize);
			return JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
		}
		public async Task<byte[]> GetByteArrayAsync(string path, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			var content = await GetAsyncInternal(path, parameters, authorize);
			return await content.ReadAsByteArrayAsync();
		}

		//public async Task PostAsync(string path, byte[] item, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		//{
		//	if (authorize)
		//	{
		//		await CheckTokenLifetimeAsync();
		//	}

		//	Uri uri = new Uri(CreateUri(path, parameters), UriKind.Relative);

		//	HttpContent content = new ByteArrayContent(item);
		//	content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
		//	var response = await _client.PostAsync(uri, content);

		//	if (!response.IsSuccessStatusCode)
		//	{
		//		throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
		//	}
		//}

		public async Task PostAsync<T>(string path, T item, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			await PostAsyncInternal(path, item, parameters, authorize);
		}
		public async Task<TRes> PostAsync<T, TRes>(string path, T item, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			var resultString = await PostAsyncInternal(path, item, parameters, authorize);
			return JsonConvert.DeserializeObject<TRes>(resultString);
		}

		public async Task PutAsync<T>(string path, T item, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			if (authorize)
			{
				await CheckTokenLifetimeAsync();
			}

			Uri uri = new Uri(CreateUri(path, parameters), UriKind.Relative);

			string json = JsonConvert.SerializeObject(item);
			HttpContent content = new StringContent(json);
			content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = await _client.PutAsync(uri, content);

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
			}
		}

		public async Task DeleteAsync<TKey>(string path, TKey id, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			if (authorize)
			{
				await CheckTokenLifetimeAsync();
			}

			Uri uri = new Uri(CreateUri(path + id, parameters), UriKind.Relative);

			var response = await _client.DeleteAsync(uri);

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
			}
		}

		private async Task<HttpContent> GetAsyncInternal(string path, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			if (authorize)
			{
				await CheckTokenLifetimeAsync();
			}

			Uri uri = new Uri(CreateUri(path, parameters), UriKind.Relative);

			var response = await _client.GetAsync(uri);
			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
			}

			return response.Content;
		}

		private async Task<string> PostAsyncInternal<T>(string path, T item, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			if (authorize)
			{
				await CheckTokenLifetimeAsync();
			}

			Uri uri = new Uri(CreateUri(path, parameters), UriKind.Relative);

			string json = JsonConvert.SerializeObject(item);
			HttpContent content = new StringContent(json);
			content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = await _client.PostAsync(uri, content);

			if (!response.IsSuccessStatusCode)
			{
				throw new InvalidResponseException(response.StatusCode + ": " + response.ReasonPhrase);
			}

			return await response.Content.ReadAsStringAsync();
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

				LoginModel model = new LoginModel() { Username = username, Password = password };
				await LoginAsync(model);
			}
		}

		private async Task SetCredentialsAsync(string username, string password, JwtToken token)
		{
			await SecureStorage.SetAsync("username", username);
			Username = username;
			await SecureStorage.SetAsync("password", password);
			await SecureStorage.SetAsync("token", token.TokenString);
			await SecureStorage.SetAsync("token_expire", token.ExpireDateTime.ToString(CultureInfo.InvariantCulture));

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.TokenString);
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
			_builder.Remove(_builder.Length - 1, 1);

			return _builder.ToString();
		}

		private class JwtToken
		{
			public string TokenString { get; set; }
			public DateTime ExpireDateTime { get; set; }
		}
	}
}
