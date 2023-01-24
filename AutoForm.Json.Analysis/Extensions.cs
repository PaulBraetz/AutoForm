using System;
using System.Linq;

using AutoForm.Analysis;

using RhoMicro.CodeAnalysis;

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
					nameof(ModelSpace.Controls),
					JsonDecorator<Control>.ObjectArray(
						modelSpace.Controls,
						JsonMembers)),
				JsonDecorator<Template[]>.KeyValuePair(
					nameof(ModelSpace.Templates),
					JsonDecorator<Template>.ObjectArray(
						modelSpace.Templates,
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
					JsonDecorator<ITypeIdentifier>.String(model.Name)),
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Model.Control),
					JsonDecorator<ITypeIdentifier>.String(model.Control)),
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Model.Template),
					JsonDecorator<ITypeIdentifier>.String(model.Template)),
				JsonDecorator<ITypeIdentifier[]>.KeyValuePair(
					nameof(Model.BaseModels),
					JsonDecorator<ITypeIdentifier>.StringArray(model.BaseModels)),
				JsonDecorator<Property[]>.KeyValuePair(
					nameof(Model.Properties),
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
					JsonDecorator<ITypeIdentifier>.String(control.Name)),
				JsonDecorator<ITypeIdentifier[]>.KeyValuePair(
					nameof(Control.Models),
					JsonDecorator<ITypeIdentifier>.StringArray(
						control.Models)),
				JsonDecorator<PropertyIdentifier[]>.KeyValuePair(
					nameof(Control.Properties),
					JsonDecorator<PropertyIdentifier>.StringArray(
						control.Properties,
						p=>p.ToLongString())),
			};

			return members;
		}
		private static IJson[] JsonMembers(this Template template)
		{
			var members = new IJson[]
			{
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Template.Name),
					JsonDecorator<ITypeIdentifier>.String(template.Name)),
				JsonDecorator<ITypeIdentifier[]>.KeyValuePair(
					nameof(Template.Models),
					JsonDecorator<ITypeIdentifier>.StringArray(
						template.Models)),
				JsonDecorator<PropertyIdentifier[]>.KeyValuePair(
					nameof(Control.Properties),
					JsonDecorator<PropertyIdentifier>.StringArray(
						template.Properties,
						p=>p.ToLongString())),
			};

			return members;
		}
		private static IJson[] JsonMembers(this Property property)
		{
			var members = new IJson[]
			{
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Property.Control),
					JsonDecorator<ITypeIdentifier>.String(property.Control)),
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Property.Template),
					JsonDecorator<ITypeIdentifier>.String(property.Template)),
				JsonDecorator<PropertyIdentifier>.KeyValuePair(
					nameof(Property.Name),
					JsonDecorator<PropertyIdentifier>.String(property.Name)),
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					"Location",
					JsonDecorator<ITypeIdentifier>.String(property.Name.Model)),
				JsonDecorator<ITypeIdentifier>.KeyValuePair(
					nameof(Property.Type),
					JsonDecorator<ITypeIdentifier>.String(property.Type))
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
