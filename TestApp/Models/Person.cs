using TestApp.Controls;

namespace TestApp.Models
{

	[AutoControlModel]
	public sealed class Person
	{
		[AutoControlModel]
		public sealed class Address
		{
			public String? Street { get; set; }
			public String? City { get; set; }
			[AutoControlAttributesProvider]
			public AddressAttributesProvider AttributesProvider { get; } = new();
		}

		public Person()
		{
			Location1 = new();
			Location2 = new();
		}
		[AutoControlPropertyPosition(1)]
		[AutoControlModelProperty(typeof(TestApp.Controls.AddressControl))]
		public Address Location1 { get; set; }

		[AutoControlModelProperty(typeof(TestApp.Controls.AddressControl))]
		public Address Location2 { get; set; }

		[AutoControlModelProperty(typeof(TestApp.Controls.PersonControl))]
		public Person? Dad { get; set; }

		[AutoControlPropertyPosition(-1)]
		[AutoControlModelProperty(typeof(TestApp.Controls.PersonControl))]
		public Person? Mom { get; set; }

		[AutoControlAttributesProvider]
		public PersonAttributesProvider AttributeProvider { get; } = new();
	}
	public sealed class PersonAttributesProvider
	{
		private readonly Dictionary<String, Object> _default = new()
		{
			{"class", "input-group" }
		};

		private readonly Dictionary<String, Object> _location1 = new()
		{
			{"class", "input-group" },
			{"label",nameof(Person.Location1) }
		};
		public Dictionary<String, Object> GetLocation1Attributes() => _location1;

		private readonly Dictionary<String, Object> _location2 = new()
		{
			{"class", "input-group" },
			{"label",nameof(Person.Location2) }
		};
		public Dictionary<String, Object> GetLocation2Attributes() => _location2;

		private readonly Dictionary<String, Object> _dad = new()
		{
			{"class", "input-group" },
			{"label",nameof(Person.Dad) }
		};
		public Dictionary<String, Object> GetDadAttributes() => _dad;

		private readonly Dictionary<String, Object> _mom = new()
		{
			{"class", "input-group" },
			{"label",nameof(Person.Mom) }
		};
		public Dictionary<String, Object> GetMomAttributes() => _mom;
	}
	public sealed class AddressAttributesProvider
	{
		private static readonly Dictionary<String, Object> _streetAttributes = new()
		{
			{"placeholder", nameof(Person.Address.Street) },
			{"class", "form-control" }
		};
		private static readonly Dictionary<String, Object> _cityAttributes = new()
		{
			{"placeholder", nameof(Person.Address.City) },
			{"class", "form-control" }
		};

		public Dictionary<String, Object> GetStreetAttributes()
		{
			return _streetAttributes;
		}
		public Dictionary<String, Object> GetCityAttributes()
		{
			return _cityAttributes;
		}
	}
}
