using System;
using System.Collections.Generic;

using RhoMicro.CodeAnalysis;

namespace AutoForm.Analysis
{
	internal readonly struct PropertyIdentifier : IEquatable<PropertyIdentifier>
	{
		public readonly String Name;
		public readonly ITypeIdentifier Model;

		private PropertyIdentifier(String name, ITypeIdentifier model) : this()
		{
			Name = name;
			Model = model;
		}

		public static PropertyIdentifier Create(String name, ITypeIdentifier model) => new PropertyIdentifier(name, model);

		public PropertyIdentifier WithModel(ITypeIdentifier model) => new PropertyIdentifier(Name, model);

		public override Boolean Equals(Object obj) => obj is PropertyIdentifier identifier && Equals(identifier);

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

		public static Boolean operator ==(PropertyIdentifier left, PropertyIdentifier right) => left.Equals(right);

		public static Boolean operator !=(PropertyIdentifier left, PropertyIdentifier right) => !(left == right);
		public override String ToString() => Name;
		public String ToLongString() => $"{Model}.{Name}";
	}
}
