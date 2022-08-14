using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate.Models
{
	public readonly struct Model : IEquatable<Model>
	{
		public readonly TypeIdentifier Name;
		public readonly IEnumerable<Property> Properties;
		public readonly PropertyIdentifier AttributesProviderIdentifier;
		private readonly String _stringRepresentation;

		private Model(TypeIdentifier identifier, IEnumerable<Property> properties, PropertyIdentifier attributesProvider)
		{
			Name = identifier;
			Properties = properties;
			AttributesProviderIdentifier = attributesProvider;

			_stringRepresentation = String.IsNullOrEmpty(Name.ToString()) ?
									String.Empty :
									$"{Name}\n{{\n{String.Join(";\n", Properties.Select(p => p.ToString()).Append(AttributesProviderIdentifier.ToString()).Where(s => !String.IsNullOrEmpty(s)))}\n}}";
		}

		public static Model Create(TypeIdentifier identifier, PropertyIdentifier attributesProvider)
		{
			return new Model(identifier, Array.Empty<Property>(), attributesProvider);
		}
		public static Model Create(TypeIdentifier identifier)
		{
			return Create(identifier, default);
		}
		public Model Append(Property property)
		{
			return new Model(Name, Properties.Append(property), AttributesProviderIdentifier);
		}
		public Model AppendRange(IEnumerable<Property> properties)
		{
			return new Model(Name, Properties.Concat(properties), AttributesProviderIdentifier);
		}

		public override Boolean Equals(Object obj)
		{
			return obj is Model type && Equals(type);
		}

		public Boolean Equals(Model other)
		{
			return _stringRepresentation == other._stringRepresentation;
		}

		public override Int32 GetHashCode()
		{
			return -992964542 + EqualityComparer<String>.Default.GetHashCode(_stringRepresentation);
		}

		public override String ToString()
		{
			return _stringRepresentation ?? String.Empty;
		}

		public static Boolean operator ==(Model left, Model right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(Model left, Model right)
		{
			return !(left == right);
		}
	}
}
