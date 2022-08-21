using AutoForm.Attributes;
using AutoForm.Blazor.Templates;
using System.Collections.ObjectModel;
using static TestApp.Models.AttributesFactory;

namespace TestApp.Models
{
	[Model]
	public interface IModel
	{
		[UseControl(typeof(AutoForm.Blazor.Controls.ByteRange))]
		[UseTemplate(typeof(AutoForm.Blazor.Templates.Empty))]
		public Byte Value { get; set; }
	}
	public class Model : IModel
	{
		private Byte _value;
		public Byte Value { get => _value; set => Console.WriteLine(this._value= value); }
		public static IModel Create()
		{
			return new Model();
		}
	}
}
