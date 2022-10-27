using System;
using System.Collections.Generic;

namespace AutoForm.Analysis
{
	internal readonly struct PropertyIdentifier : IEquatable<PropertyIdentifier>
	{
		public readonly string Name;

		private PropertyIdentifier(string name) : this()
		{
			Name = name;
		}

		public static PropertyIdentifier Create(string name)
		{
			return new PropertyIdentifier(name);
		}

		public override String ToString()
		{
			return Name;
		}

		public override Boolean Equals(Object obj)
		{
			return obj is PropertyIdentifier identifier && Equals(identifier);
		}

		public Boolean Equals(PropertyIdentifier other)
		{
			return Name == other.Name;
		}

		public override Int32 GetHashCode()
		{
			return 539060726 + EqualityComparer<String>.Default.GetHashCode(Name);
		}

		public static Boolean operator ==(PropertyIdentifier left, PropertyIdentifier right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(PropertyIdentifier left, PropertyIdentifier right)
		{
			return !(left == right);
		}
	}
}
