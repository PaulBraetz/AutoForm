using System;
using System.Collections.Generic;
using System.Linq;

using RhoMicro.CodeAnalysis;

namespace AutoForm.Analysis
{
	internal static class Extensions
	{
		public static void ThrowOnDuplicate<T>(this IEnumerable<T> values, String name)
		{
			var comparer = EqualityComparer<T>.Default;
			var duplicate = values.FirstOrDefault(v1 => values.Where(v2 => comparer.Equals(v1, v2)).Count() > 1);
			if(!comparer.Equals(duplicate, default))
			{
				throw new ArgumentException($"Cannot register {name} {duplicate} multiple times.");
			}
		}
		public static ITypeIdentifier AsGenerated(this ITypeIdentifier identifier)
		{
			var generatedName = $"__Control_{identifier.ToString().Replace(".", "_")}";
			var name = TypeIdentifierName.Create().AppendNamePart(generatedName);
			var @namespace = Namespace.Create();
			var generated = new GeneratedTypeIdentifierDecorator(TypeIdentifier.Create(name, @namespace));

			return generated;
		}
		public static Boolean IsGenerated(this ITypeIdentifier identifier)
		{
			var isGenerated = identifier is GeneratedTypeIdentifierDecorator;

			return isGenerated;
		}
	}
}
