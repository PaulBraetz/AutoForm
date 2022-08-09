namespace TestApp.Models
{
	namespace NestedNamespace
	{
		[AutoControlModel]
		public class MyModel
		{
			public String? Name { get; set; }
			public Int32? ID { get; set; }
		}
		[AutoControlModel]
		public class MyOtherModel
		{
			public String? Name { get; set; }
			public Int32? ID { get; set; }
		}
	}
}
