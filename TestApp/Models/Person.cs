namespace TestApp.Models
{
	[AutoControlModel]
	internal sealed class Address
	{
		public String Street { get; set; }
		public String City { get; set; }

		public override String ToString()
		{
			return $@"{{Street = ""{Street}"", City = ""{City}""}}";
		}
	}
	[AutoControlModel]
	internal sealed class Person
	{
		public String Name { get; set; }
		public String LastName { get; set; }
		public Address Address { get; set; } = new Address();

		public override String ToString()
		{
			return $@"{{Name = ""{Name}"", LastName = ""{LastName}"", Address = {Address}}}";
		}
	}
}
