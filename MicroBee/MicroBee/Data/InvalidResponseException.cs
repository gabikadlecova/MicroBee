using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace MicroBee.Data
{
	/// <summary>
	/// Is thrown when the http request has not succeeded (e.g. not by 200 Ok)
	/// </summary>
	class InvalidResponseException : HttpRequestException
	{
		public InvalidResponseException()
		{
		}

		public InvalidResponseException(string message) : base(message)
		{
		}

		public InvalidResponseException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
