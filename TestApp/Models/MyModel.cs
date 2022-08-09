namespace TestApp.Models
{
	namespace NestedNamespace
	{
		[AutoControlModel]
		public class MyModel
		{
			public MyModel? InnerModel { get; set; }
			public String? Name { get; set; }

			public override String ToString()
			{
				var innerString = InnerModel?.ToString() ?? "Null";
				return $"{{Name: {Name}, Inner: {innerString}}}";
			}
		}
	}
}
