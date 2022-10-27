using AutoForm.Blazor.Attributes;

namespace TestApp.AttributesProviders
{
	public class MyAttributesProvider
	{
		public IEnumerable<KeyValuePair<String, Object>> GetNameAttributes()
		{
			return new AttributeCollection(("placeholder", "Name"), ("class", "form-control"), ("label", "User Name"));
		}
		public IEnumerable<KeyValuePair<String, Object>> GetAgeAttributes()
		{
			return new AttributeCollection(("min", "0"), ("class", "form-control"), ("label", "User Age"));
		}
	}
}
