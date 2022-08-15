using System;
using System.Collections.Generic;

namespace AutoForm.Generate.Models
{
	public readonly struct ModelSpace : IEquatable<ModelSpace>
	{
		public readonly IEnumerable<Template> Templates;
		public readonly IEnumerable<Model> Models;
		public readonly IEnumerable<Control> Controls;
		private readonly String _stringRepresentation;

		private ModelSpace(IEnumerable<Model> models, IEnumerable<Control> controls, IEnumerable<Template> templates)
		{
			models.ThrowOnDuplicate(TypeIdentifierName.AutoControlModelAttribute.ToString());
			controls.ThrowOnDuplicate(TypeIdentifierName.AutoControlAttribute.ToString());
			templates.ThrowOnDuplicate(TypeIdentifierName.AutoControlTemplateAttribute.ToString());

			Models = models;
			Controls = controls;
			Templates = templates;

			_stringRepresentation = Json.Object(Json.KeyValuePair(nameof(Models), Models),
												Json.KeyValuePair(nameof(Controls), Controls),
												Json.KeyValuePair(nameof(Templates), Templates));
		}

		public static ModelSpace Create(IEnumerable<Model> models, IEnumerable<Control> controls, IEnumerable<Template> templates)
		{
			return new ModelSpace(models, controls, templates);
		}

		public override String ToString()
		{
			return ToJson();
		}

		public String ToJson()
		{
			return _stringRepresentation ?? "null";
		}
		public override Boolean Equals(Object obj)
		{
			return obj is ModelSpace space && Equals(space);
		}

		public Boolean Equals(ModelSpace other)
		{
			return _stringRepresentation == other._stringRepresentation;
		}

		public override Int32 GetHashCode()
		{
			return -992964542 + EqualityComparer<String>.Default.GetHashCode(_stringRepresentation);
		}

		public static Boolean operator ==(ModelSpace left, ModelSpace right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(ModelSpace left, ModelSpace right)
		{
			return !(left == right);
		}
	}
}
