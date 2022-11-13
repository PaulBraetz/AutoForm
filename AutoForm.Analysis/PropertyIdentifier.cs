using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace AutoForm.Analysis
{
	internal readonly struct PropertyIdentifier : IEquatable<PropertyIdentifier>
	{
		public readonly string Name;
		public readonly ITypeIdentifier Model;

		private PropertyIdentifier(string name, ITypeIdentifier model) : this()
		{
			Name = name;
			Model = model;
		}

		public static PropertyIdentifier Create(string name, ITypeIdentifier model)
		{
			return new PropertyIdentifier(name, model);
		}

		public PropertyIdentifier WithModel(ITypeIdentifier model)
		{
			return new PropertyIdentifier(Name, model);
		}

		public override Boolean Equals(Object obj)
		{
			return obj is PropertyIdentifier identifier && Equals(identifier);
		}

		public Boolean Equals(PropertyIdentifier other)
		{
			return Name == other.Name &&
				TypeIdentifierEqualityComparer.Instance.Equals(Model, other.Model);
		}

		public override Int32 GetHashCode()
		{
			var hashCode = -1566092560;
			hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(Name);
			hashCode = hashCode * -1521134295 + TypeIdentifierEqualityComparer.Instance.GetHashCode(Model);
			return hashCode;
		}

		public static Boolean operator ==(PropertyIdentifier left, PropertyIdentifier right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(PropertyIdentifier left, PropertyIdentifier right)
		{
			return !(left == right);
		}
		public override String ToString()
		{
			return Name;
		}
		public String ToLongString()
		{
			return $"{Model}.{Name}";
		}
	}
}
