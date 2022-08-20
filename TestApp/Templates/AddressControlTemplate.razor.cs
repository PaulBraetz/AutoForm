using AutoForm.Attributes;
using TestApp.Models;

namespace TestApp.Templates
{
	[FallbackTemplate(typeof(Address))]
	public partial class AddressControlTemplate
	{
	}
}
