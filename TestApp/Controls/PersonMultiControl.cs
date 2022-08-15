using TestApp.Models;

namespace TestApp.Controls
{
	[AutoForm.Attributes.AutoControl(typeof(ICollection<TestApp.Models.Person>))]
	[AutoForm.Attributes.AutoControl(typeof(List<TestApp.Models.Person>))]
	public class PersonMultiControl : CtorMultiControlBase<Person>
	{
	}
}
