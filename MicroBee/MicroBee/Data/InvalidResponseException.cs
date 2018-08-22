using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace MicroBee.Data
{
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
