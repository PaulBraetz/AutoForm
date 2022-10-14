using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis
{
	internal readonly struct Error : IEquatable<Error>
	{
		private Error(Exception[] exceptions)
		{
			Exceptions = exceptions ?? Array.Empty<Exception>();

			_string =
$@"/*
An error has occured:
{String.Join("\n\n", exceptions.Select(e => e.ToString()))}
*/";
		}

		public readonly Exception[] Exceptions;
		private readonly string _string;

		public static Error Create()
		{
			return new Error(Array.Empty<Exception>());
		}
		public Error With(Exception exception)
		{
			return new Error(Exceptions.Append(exception).ToArray());
		}

		public override String ToString()
		{
			return _string;
		}

		public override Boolean Equals(Object obj)
		{
			return obj is Error error && Equals(error);
		}

		public Boolean Equals(Error other)
		{
			return _string == other._string;
		}

		public override Int32 GetHashCode()
		{
			return -219028617 + EqualityComparer<String>.Default.GetHashCode(_string);
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
