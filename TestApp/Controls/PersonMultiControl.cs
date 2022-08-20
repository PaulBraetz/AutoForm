using TestApp.Models;

namespace TestApp.Controls
{
	[AutoForm.Attributes.FallbackControl(typeof(ICollection<TestApp.Models.Person>))]
	[AutoForm.Attributes.FallbackControl(typeof(List<TestApp.Models.Person>))]
	public class PersonMultiControl : CtorMultiControlBase<Person>
	{
	}
}
