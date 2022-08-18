using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis.Models
{
	public readonly struct FallbackControl : IEquatable<FallbackControl>
	{
		public readonly TypeIdentifier Name;
		public readonly IEnumerable<TypeIdentifier> Models;
		private readonly String _json;
		private readonly String _string;

		private FallbackControl(TypeIdentifier name, IEnumerable<TypeIdentifier> models)
		{
			models.ThrowOnDuplicate(TypeIdentifierName.FallbackControlAttribute.ToString());

			Name = name;
			Models = models;

			_json = Json.Object(Json.KeyValuePair(nameof(Name), Name),
								Json.KeyValuePair(nameof(Models), Models));
			_string = _json;
		}

		public static FallbackControl Create(Model model)
		{
			var name = TypeIdentifierName.Create().AppendNamePart($"__Control_{model.Name.ToEscapedString().Replace('.', '_')}");
			var @namespace = Namespace.Create();

			var identifier = TypeIdentifier.Create(name, @namespace);

			var control = Create(identifier)
				.Append(model.Name);

			return control;
		}
		public static FallbackControl Create(TypeIdentifier identifier)
		{
			return new FallbackControl(identifier, Array.Empty<TypeIdentifier>());
		}
		public FallbackControl Append(TypeIdentifier modelIdentifier)
		{
			return new FallbackControl(Name, Models.Append(modelIdentifier));
		}
		public FallbackControl AppendRange(IEnumerable<TypeIdentifier> modelIdentifiers)
		{
			return new FallbackControl(Name, Models.Concat(modelIdentifiers));
		}

		public override Boolean Equals(Object obj)
		{
			return obj is FallbackControl control && Equals(control);
		}

		public Boolean Equals(FallbackControl other)
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

		public static Boolean operator ==(FallbackControl left, FallbackControl right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(FallbackControl left, FallbackControl right)
		{
			return !(left == right);
		}
	}
}
