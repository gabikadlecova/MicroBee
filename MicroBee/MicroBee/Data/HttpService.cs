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
	public class HttpService : IDisposable
	{
		private readonly HttpClient _client;
		private readonly StringBuilder _builder = new StringBuilder();

		private readonly TimeSpan _timeEpsilon = new TimeSpan(0, 0, 0, 0, 100);
		/// <summary>
		/// Where the login data is sent to
		/// </summary>
		private string LoginPath { get; }
		/// <summary>
		/// Where the register data is sent to
		/// </summary>
		private string RegisterPath { get; }

		/// <summary>
		/// Returns true if the user has been logged in
		/// </summary>
		public bool Authenticated { get; private set; }
		/// <summary>
		/// Returns the username of the current user
		/// </summary>
		public string Username { get; private set; }


		public HttpService(string host, string loginPath, string registerPath)
		{
			_client = new HttpClient { BaseAddress = new Uri(host) };

			LoginPath = loginPath;
			RegisterPath = registerPath;

			Authenticated = false;
		}

		/// <summary>
		/// Tries to log the user in with login data previously saved in the device storage.
		/// </summary>
		/// <returns>True if the login operation succeeded</returns>
		public async Task<bool> TryLoginAsync()
		{
			string username = await SecureStorage.GetAsync("username");
			string password = await SecureStorage.GetAsync("password");

			//one of the credentials is missing
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				return false;
			}

			await LoginAsync(new LoginModel()
			{
				Username = username,
				Password = password
			});

			return true;
		}

		public async Task LoginAsync(LoginModel model)
		{
			//Previously saved data is erased
			Logout();

			//login (by jwt bearer token)
			JwtToken token = await PostAsync<LoginModel, JwtToken>(LoginPath, model);
			await SetCredentialsAsync(model.Username, model.Password, token);

			Authenticated = true;
		}

		/// <summary>
		/// Logs the user out by deleting data in the device
		/// </summary>
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

		/// <summary>
		/// Registers a new user
		/// </summary>
		/// <param name="model">Register data</param>
		/// <returns></returns>
		public async Task RegisterAsync(RegisterModel model)
		{
			Logout();

			JwtToken token = await PostAsync<RegisterModel, JwtToken>(RegisterPath, model);
			await SetCredentialsAsync(model.Username, model.Password, token);

			Authenticated = true;
		}

		/// <summary>
		/// Gets an object of the type T from the specified path
		/// </summary>
		/// <typeparam name="T">Type of the result</typeparam>
		/// <param name="path">Path where the request will be sent to</param>
		/// <param name="parameters">Request parameters</param>
		/// <param name="authorize">Specifies whether the path requires user authorization</param>
		/// <returns>Object of type T</returns>
		public async Task<T> GetAsync<T>(string path, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			var content = await GetAsyncInternal(path, parameters, authorize);
			return JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
		}
		/// <summary>
		/// Gets a byte array from the specified path
		/// </summary>
		/// <param name="path">Path where the request will be sent to</param>
		/// <param name="parameters">Request parameters</param>
		/// <param name="authorize">Specifies whether the path requires user authorization</param>
		/// <returns>A byte array</returns>
		public async Task<byte[]> GetByteArrayAsync(string path, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			var content = await GetAsyncInternal(path, parameters, authorize);
			return await content.ReadAsByteArrayAsync();
		}

		/// <summary>
		/// Posts an object of type T to the specified path and dismisses the result data.
		/// </summary>
		/// <typeparam name="T">Type of the sent object</typeparam>
		/// <param name="path">Path where the request will be sent to</param>
		/// <param name="item">Object of type T which will be sent in the request</param>
		/// <param name="parameters">Request parameters</param>
		/// <param name="authorize">Specifies whether the path requires user authorization</param>
		/// <returns></returns>
		public async Task PostAsync<T>(string path, T item, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			await PostAsyncInternal(path, item, parameters, authorize);
		}
		/// <summary>
		/// Posts an object of type T to the specified path and keeps the response data.
		/// </summary>
		/// <typeparam name="T">Type of the sent object</typeparam>
		/// <typeparam name="TRes">Type of the result</typeparam>
		/// <param name="path">Path where the request will be sent to</param>
		/// <param name="item">Object of type T which will be sent in the request</param>
		/// <param name="parameters">Request parameters</param>
		/// <param name="authorize">Specifies whether the path requires user authorization</param>
		/// <returns>Object of type TRes sent in the response</returns>
		public async Task<TRes> PostAsync<T, TRes>(string path, T item, List<KeyValuePair<string, object>> parameters = null, bool authorize = false)
		{
			var resultString = await PostAsyncInternal(path, item, parameters, authorize);
			return JsonConvert.DeserializeObject<TRes>(resultString);
		}

		/// <summary>
		/// Puts an object of type T to the specified path, does not expect to have a result returned
		/// </summary>
		/// <typeparam name="T">Type of the sent object</typeparam>
		/// <param name="path">Path where the request will be sent to</param>
		/// <param name="item">Object of type T which will be sent in the request</param>
		/// <param name="parameters">Request parameters</param>
		/// <param name="authorize">Specifies whether the path requires user authorization</param>
		/// <returns></returns>
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

		/// <summary>
		/// Deletes data on path specified by TKey
		/// </summary>
		/// <typeparam name="TKey">Type of the data key</typeparam>
		/// <param name="path">Path where the request will be sent to</param>
		/// <param name="id">ID of the data to be deleted</param>
		/// <param name="parameters">Request parameters</param>
		/// <param name="authorize">Specifies whether the path requires user authorization</param>
		/// <returns></returns>
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

		/// <summary>
		/// Checks if the token needs to be refreshed; throws NotAuthenticatedException if the user is not logged in
		/// </summary>
		/// <returns></returns>
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

				//gets a new token
				LoginModel model = new LoginModel() { Username = username, Password = password };
				await LoginAsync(model);
			}
		}

		/// <summary>
		/// Stores user credentials to device secure persistent storage
		/// </summary>
		/// <param name="username">Username data</param>
		/// <param name="password">Password data</param>
		/// <param name="token">Current bearer token</param>
		/// <returns></returns>
		private async Task SetCredentialsAsync(string username, string password, JwtToken token)
		{
			await SecureStorage.SetAsync("username", username);
			Username = username;
			await SecureStorage.SetAsync("password", password);
			await SecureStorage.SetAsync("token", token.TokenString);
			await SecureStorage.SetAsync("token_expire", token.ExpireDateTime.ToString(CultureInfo.InvariantCulture));

			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.TokenString);
		}

		/// <summary>
		/// Creates a Uri string by combining the path and route parameters. No parameters are added to the uri if the
		/// list is null or empty.
		/// </summary>
		/// <param name="path">Uri path without parameters</param>
		/// <param name="parameters">Route parameter dictionary</param>
		/// <returns>Uri string</returns>
		private string CreateUri(string path, List<KeyValuePair<string, object>> parameters)
		{
			if (parameters == null || parameters.Count == 0)
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

		/// <summary>
		/// Jwt token model
		/// </summary>
		private class JwtToken
		{
			public string TokenString { get; set; }
			public DateTime ExpireDateTime { get; set; }
		}

		public void Dispose()
		{
			_client?.Dispose();
		}
	}
}
