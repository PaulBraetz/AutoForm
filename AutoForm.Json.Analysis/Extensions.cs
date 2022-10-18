using AutoForm.Analysis;
using RhoMicro.CodeAnalysis;
using System;
using System.Linq;

namespace AutoForm.Json.Analysis
{
	internal static class Extensions
	{
		public static JsonDecorator<ModelSpace> ToJson(this ModelSpace modelSpace)
		{
			var decorator = JsonDecorator<ModelSpace>.Object(modelSpace, JsonMembers);

			return decorator;
		}
		private static IJson[] JsonMembers(this ModelSpace modelSpace)
		{
			var members = new IJson[]
			{
				JsonDecorator<Model[]>.KeyValuePair(
					nameof(ModelSpace.Models),
					JsonDecorator<Model>.ObjectArray(
						modelSpace.Models,
						JsonMembers)),
				JsonDecorator<Control[]>.KeyValuePair(
					nameof(ModelSpace.FallbackControls),
					JsonDecorator<Control>.ObjectArray(
						modelSpace.FallbackControls,
						JsonMembers)),
				JsonDecorator<Template[]>.KeyValuePair(
					nameof(ModelSpace.FallbackTemplates),
					JsonDecorator<Template>.ObjectArray(
						modelSpace.FallbackTemplates,
						JsonMembers)),
				JsonDecorator<Control[]>.KeyValuePair(
					nameof(ModelSpace.RequiredGeneratedControls),
					JsonDecorator<Control>.ObjectArray(
						modelSpace.RequiredGeneratedControls,
						JsonMembers))
			};

			return members;
		}
		private static IJson[] JsonMembers(this Model model)
		{
			var members = new IJson[]
			{
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Model.Name),
					JsonDecorator<ITypeIdentifier>.Object(
						model.Name,
						JsonMembers)),
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Model.Control),
					JsonDecorator<ITypeIdentifier>.Object(
						model.Control,
						JsonMembers)),
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Model.Template),
					JsonDecorator<ITypeIdentifier>.Object(
						model.Template,
						JsonMembers)),
				JsonDecorator<PropertyIdentifier>.KeyValuePair(
					nameof(Model.AttributesProvider),
					JsonDecorator<PropertyIdentifier>.Object(
						model.AttributesProvider,
						JsonMembers)),
				JsonDecorator<Property[]>.KeyValuePair(
					nameof(Model.AttributesProvider),
					JsonDecorator<Property>.ObjectArray(
						model.Properties,
						JsonMembers))
			};

			return members;
		}
		private static IJson[] JsonMembers(this Control control)
		{
			var members = new IJson[]
			{
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Control.Name),
					JsonDecorator<ITypeIdentifier>.Object(
						control.Name,
						JsonMembers)),
				JsonDecorator<ITypeIdentifier[]>.KeyValuePair(
					nameof(Control.Models),
					JsonDecorator<ITypeIdentifier>.ObjectArray(
						control.Models,
						JsonMembers))
			};

			return members;
		}
		private static IJson[] JsonMembers(this Template template)
		{
			var members = new IJson[]
			{
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Template.Name),
					JsonDecorator<ITypeIdentifier>.Object(
						template.Name,
						JsonMembers)),
				JsonDecorator<ITypeIdentifier[]>.KeyValuePair(
					nameof(Template.Models),
					JsonDecorator<ITypeIdentifier>.ObjectArray(
						template.Models,
						JsonMembers))
			};

			return members;
		}
		private static IJson[] JsonMembers(this ITypeIdentifier identifier)
		{
			var members = new IJson[]
			{
				JsonDecorator<INamespace>.KeyValuePair(
					nameof(ITypeIdentifier.Namespace),
					JsonDecorator<INamespace>.Object(
						identifier.Namespace,
						JsonMembers)),
				JsonDecorator<ITypeIdentifierName>.KeyValuePair(
					nameof(ITypeIdentifier.Name),
					JsonDecorator<ITypeIdentifierName>.Object(
						identifier.Name,
						JsonMembers))
			};

			return members;
		}
		private static IJson[] JsonMembers(this PropertyIdentifier propertyIdentifier)
		{
			var members = new IJson[]
			{
				JsonDecorator<String>.KeyValuePair(
					nameof(PropertyIdentifier.Name),
					JsonDecorator<String>.String(
						propertyIdentifier.Name))
			};

			return members;
		}
		private static IJson[] JsonMembers(this Property property)
		{
			var members = new IJson[]
			{
				JsonDecorator<Int32>.KeyValuePair(
					nameof(Property.Order),
					JsonDecorator<Int32>.Number(
						property.Order)),
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Property.Control),
					JsonDecorator<ITypeIdentifier>.Object(
						property.Control,
						JsonMembers)),
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Property.Template),
					JsonDecorator<ITypeIdentifier>.Object(
						property.Template,
						JsonMembers)),
				JsonDecorator<PropertyIdentifier>.KeyValuePair(
					nameof(Property.Name),
					JsonDecorator<PropertyIdentifier>.Object(
						property.Name,
						JsonMembers)),
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Property.Type),
					JsonDecorator<ITypeIdentifier>.Object(
						property.Type,
						JsonMembers))
			};

			return members;
		}
		private static IJson[] JsonMembers(this ITypeIdentifierName identifierName)
		{
			var members = new IJson[]
			{
				JsonDecorator<IIdentifierPart[]>.KeyValuePair(
					nameof(ITypeIdentifierName.Parts),
					JsonDecorator<IIdentifierPart>.ObjectArray(identifierName.Parts.ToArray(), JsonMembers))
			};

			return members;
		}
		private static IJson[] JsonMembers(this INamespace @namespace)
		{
			var members = new IJson[]
			{
				JsonDecorator<IIdentifierPart[]>.KeyValuePair(
					nameof(INamespace.Parts),
					JsonDecorator<IIdentifierPart>.ObjectArray(@namespace.Parts.ToArray(), JsonMembers))
			};

			return members;
		}
		private static IJson[] JsonMembers(this IIdentifierPart identifierPart)
		{
			var members = new IJson[]
			{
				JsonDecorator<IdentifierParts.Kind>.KeyValuePair(
					nameof(IIdentifierPart.Kind),
					JsonDecorator<IdentifierParts.Kind>.String(identifierPart.Kind)),
				JsonDecorator<String>.KeyValuePair(
					nameof(IIdentifierPart.Value),
					JsonDecorator<String>.String(identifierPart.Value))
			};

			return members;
		}

		public static JsonDecorator<Error> ToJson(this Error error)
		{
			var decorator = JsonDecorator<Error>.Object(error, JsonMembers);

			return decorator;
		}
		private static IJson[] JsonMembers(this Error error)
		{
			var members = new IJson[]
			{
				JsonDecorator<Exception[]>.KeyValuePair(
					nameof(Error.Exceptions),
					JsonDecorator<Exception>.ObjectArray(
						error.Exceptions.ToArray(),
						JsonMembers))
			};

			return members;
		}
		private static IJson[] JsonMembers(this Exception exception)
		{
			var members = new IJson[]
			{
				JsonDecorator<String>.KeyValuePair(
					nameof(Exception.Message),
					JsonDecorator<String>.String(
						exception.Message)),
				JsonDecorator<Type>.KeyValuePair(
					nameof(Type),
					JsonDecorator<Type>.String(
						exception.GetType()))
			};

			return members;
		}
	}
}
