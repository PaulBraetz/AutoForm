using TestApp.Models;

namespace TestApp.Controls
{
	[AutoForm.Attributes.AutoControl(typeof(ICollection<TestApp.Models.Person>))]
	public class PersonMultiControl : CtorMultiControlBase<Person>
	{
	}
}
