using System.ComponentModel;

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
		public Person() : this(true)
		{
		}
		private Person(Boolean addParents)
		{
			//if (addParents)
			//{
			//    Mom = new Person(false);
			//    Dad = new Person(false);
			//}
			//Address = new Address();

			AttributesProvider = new PersonAttributesProvider(this);
		}
		public Boolean Male { get; set; }
		public String Name { get; set; }

		[AutoControlAttributesProvider]
		public PersonAttributesProvider AttributesProvider { get; private set; }

		//public String LastName { get; set; }
		//public Address Address { get; set; }
		//public Person Dad { get; set; }
		//public Person Mom { get; set; }

		public override String ToString()
		{
			return $@"{{Name = ""{Name}"", Male = {Male}}}";//, LastName = ""{LastName}"", Address = {Address}, Mom = {Mom}, Dad = {Dad}}}";
		}
	}
	internal sealed class PersonAttributesProvider
	{
		private readonly Person _person;

		public PersonAttributesProvider(Person person)
		{
			_person = person;
		}

		private readonly Dictionary<String, Object> _longNameAttributes = new Dictionary<string, object>()
		{
			{"class","text-danger"}
		};
		private readonly Dictionary<String, Object> _shortNameAttributes = new Dictionary<string, object>()
		{
			{"class","text-success"}
		};
		public IDictionary<String, Object> GetNameAttributes()
		{
			return (_person.Name?.Length ?? 0) > 5 ?
				_longNameAttributes :
				_shortNameAttributes;
		}

		private readonly Dictionary<String, Object> _maleAttributes = new Dictionary<string, object>()
		{

		};
		public IDictionary<String, Object> GetMaleAttributes()
		{
			return _maleAttributes;
		}
	}
}
