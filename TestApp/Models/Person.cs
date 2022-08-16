using AutoForm.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestApp.Models
{

    [AutoControlModel]
    public sealed class Person
    {
        [AutoControlModel]
        public sealed class Address
        {
            [MaxLength(10)]
            public String? Street { get; set; }
            [MaxLength(10)]
            [AutoControlPropertyOrder(-1)]
            public String? City { get; set; }

            [AutoControlAttributesProvider]
            public AddressAttributesProvider AttributesProvider { get; } = new();
        }
        public Person()
        {
            Location1 = new();
        }
        public Address Location1 { get; set; }

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
            {"label",nameof(Person.Location1) }
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
