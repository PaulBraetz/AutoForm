using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis.Models
{
    public readonly struct Error : IEquatable<Error>
    {
        private Error(Exception[] exceptions)
        {
            Exceptions = exceptions ?? Array.Empty<Exception>();

            _json = Json.Object(Json.KeyValuePair(nameof(Exceptions), Exceptions.Select(m => m.Message)));
            _string = _json;
        }

        public readonly Exception[] Exceptions;
        private readonly String _json;
        private readonly String _string;

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
            return _json ?? "null";
        }
        public String ToEscapedString()
        {
            return _string ?? String.Empty;
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Error error && Equals(error);
        }

        public Boolean Equals(Error other)
        {
            return _json == other._json;
        }

        public override Int32 GetHashCode()
        {
            return -992964542 + EqualityComparer<String>.Default.GetHashCode(_json);
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
