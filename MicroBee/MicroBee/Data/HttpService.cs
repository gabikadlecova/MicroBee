using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MicroBee.Data
{
	class HttpService
	{
		private readonly HttpClient _client = new HttpClient();

		private bool Authenticated { get; set; }

		public HttpService()
		{
			Authenticated = false;
		}

		public async Task LoginAsync()
		{

		}

		public void Logout()
		{

		}
	}
}
