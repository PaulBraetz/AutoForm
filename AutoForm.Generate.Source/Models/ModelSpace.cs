using System;
using System.Collections.Generic;
using System.Linq;

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
			thowOnDuplicate(models, "model");
			thowOnDuplicate(controls, "control");
			thowOnDuplicate(templates, "template");

			Models = models;
			Controls = controls;
			Templates = templates;

			_stringRepresentation = String.Join("\n", Controls.Select(c => c.ToString()).Concat(Templates.Select(s => s.ToString())).Concat(Models.Select(m => m.ToString())));

			void thowOnDuplicate<T>(IEnumerable<T> values, String name)
			{
				EqualityComparer<T> comparer = EqualityComparer<T>.Default;
				T duplicate = values.FirstOrDefault(v1 => values.Where(v2 => comparer.Equals(v1, v2)).Count() > 1);
				if (!comparer.Equals(duplicate, default))
				{
					throw new ArgumentException($"Cannot register {name} {duplicate} multiple times.");
				}
			}
		}

		public static ModelSpace Create(IEnumerable<Model> models, IEnumerable<Control> controls, IEnumerable<Template> templates)
		{
			return new ModelSpace(models, controls, templates);
		}

		public override String ToString()
		{
			return _stringRepresentation ?? String.Empty;
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
