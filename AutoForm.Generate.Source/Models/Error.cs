using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate.Models
{
	public readonly struct Error : IEquatable<Error>
	{
		private Error(IEnumerable<Exception> exceptions)
		{
			Exceptions = exceptions;

			_stringRepresentation = Json.Object(Json.KeyValuePair(nameof(Exceptions), Exceptions));
		}

		public readonly IEnumerable<Exception> Exceptions;
		private readonly String _stringRepresentation;

		public static Error Create()
		{
			return new Error(Array.Empty<Exception>());
		}
		public Error Append(Exception exception)
		{
			return new Error(Exceptions.Append(exception));
		}
		public override String ToString()
		{
			return _stringRepresentation;
		}

		public override Boolean Equals(Object obj)
		{
			return obj is Error error && Equals(error);
		}

		public Boolean Equals(Error other)
		{
			return _stringRepresentation == other._stringRepresentation;
		}

		public override Int32 GetHashCode()
		{
			return -992964542 + EqualityComparer<String>.Default.GetHashCode(_stringRepresentation);
		}

		public static Boolean operator ==(Error left, Error right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(Error left, Error right)
		{
			return !(left == right);
		}
	}
}
