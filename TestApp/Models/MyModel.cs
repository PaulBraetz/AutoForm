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
		public class MyModel00
		{
			public String? Name { get; set; }
			public Int32? ID { get; set; }
		}
		[AutoControlModel]
		public class MyModel100
		{
			[AutoControlModel]
			public class MyModel200
			{
				public String? Name { get; set; }
				public Int32? ID { get; set; }
			}
			public String? Name { get; set; }
			public Int32? ID { get; set; }
		}
		[AutoControlModel]
		public class MyModel300
		{
			public String? Name { get; set; }
			public Int32? ID { get; set; }
		}
	}
}
