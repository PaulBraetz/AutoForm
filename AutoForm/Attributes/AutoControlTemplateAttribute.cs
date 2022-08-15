namespace AutoForm.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class AutoControlTemplateAttribute : Attribute
	{
		public AutoControlTemplateAttribute(Type modelType)
		{
			ModelType = modelType;
		}

		public Type ModelType { get; }
	}
}
