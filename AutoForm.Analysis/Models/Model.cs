using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis.Models
{
	public readonly struct Model : IEquatable<Model>
	{
		public readonly TypeIdentifier Name;
		public readonly TypeIdentifier Control;
		public readonly TypeIdentifier Template;
		public readonly PropertyIdentifier AttributesProvider;
		public readonly Property[] Properties;
		private readonly String _json;
		private readonly String _string;

		private Model(TypeIdentifier name, TypeIdentifier control, TypeIdentifier template, Property[] properties, PropertyIdentifier attributesProvider)
		{
			properties.ThrowOnDuplicate(nameof(properties));

			Name = name;
			Control = control;
			Template = template;
			Properties = properties ?? Array.Empty<Property>();
			AttributesProvider = attributesProvider;

			_json = Json.Object(Json.KeyValuePair(nameof(Name), Name),
								Json.KeyValuePair(nameof(AttributesProvider), AttributesProvider),
								Json.KeyValuePair(nameof(Control), Control),
								Json.KeyValuePair(nameof(Template), Template),
								Json.KeyValuePair(nameof(Properties), Properties));
			_string = _json;
		}

		public static Model Create(TypeIdentifier name, TypeIdentifier control, TypeIdentifier template, PropertyIdentifier attributesProvider)
		{
			return new Model(name, control, template, Array.Empty<Property>(), attributesProvider);
		}
		public Model With(Property property)
		{
			return new Model(Name, Control, Template, Properties.Append(property).ToArray(), AttributesProvider);
		}
		public Model WithRange(IEnumerable<Property> properties)
		{
			return new Model(Name, Control, Template, Properties.Concat(properties).ToArray(), AttributesProvider);
		}

		public override Boolean Equals(Object obj)
		{
			return obj is Model type && Equals(type);
		}

		public Boolean Equals(Model other)
		{
			return _json == other._json;
		}

		public override Int32 GetHashCode()
		{
			return -992964542 + EqualityComparer<String>.Default.GetHashCode(_json);
		}

		public override String ToString()
		{
			return _json ?? "null";
		}
		public String ToEscapedString()
		{
			return _string ?? String.Empty;
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
