using TestApp.Controls;

namespace TestApp.Models
{

    [AutoControlModel]
    public sealed class PersonList
    {
        [AutoControlModel]
        public sealed class Address
        {
            public String? Street { get; set; }
            public String? City { get; set; }
            [AutoControlAttributesProvider]
            public AddressAttributesProvider AttributesProvider { get; } = new();
        }

        public PersonList(PersonList? previous = null)
        {
            Location1 = new();
            Previous = previous;
        }
        public Address Location1 { get; set; }

        [AutoControlModelProperty(typeof(TestApp.Controls.PersonControl))]
        public PersonList? Next { get; set; }

        [AutoControlModelExclude]
        public PersonList? Previous { get; set; }

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
            {"class", "input-group my-1" },
            {"label",nameof(PersonList.Location1) }
        };
        public Dictionary<String, Object> GetLocation1Attributes() => _location1;

        private readonly Dictionary<String, Object> _person = new()
        {
            {"class", "my-1" },
            {"label","Person" }
        };
        public Dictionary<String, Object> GetNextAttributes() => _person;

    }
    public sealed class AddressAttributesProvider
    {
        private static readonly Dictionary<String, Object> _streetAttributes = new()
        {
            {"placeholder", nameof(PersonList.Address.Street) },
            {"class", "form-control" }
        };
        private static readonly Dictionary<String, Object> _cityAttributes = new()
        {
            {"placeholder", nameof(PersonList.Address.City) },
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
