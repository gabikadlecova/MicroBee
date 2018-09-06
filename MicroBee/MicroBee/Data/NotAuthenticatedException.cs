using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MicroBee.Data
{
	/// <summary>
	/// Is thrown if protected data is accessed by an anonymous (not authenticated) user
	/// </summary>
	class NotAuthenticatedException : Exception
	{
		public NotAuthenticatedException()
		{
		}

		public NotAuthenticatedException(string message) : base(message)
		{
		}

		public NotAuthenticatedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected NotAuthenticatedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
