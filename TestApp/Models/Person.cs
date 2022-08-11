using TestApp.Controls;

namespace TestApp.Models
{
    [AutoControlModel]
    public sealed class Address
    {
        public String? Street { get; set; }
        public String? City { get; set; }
        [AutoControlAttributesProvider]
        public AddressAttributesProvider AttributesProvider { get; } = new();
    }
    [AutoControlModel]
    public sealed class Person
    {
        public Person()
        {
            Address = new();
        }
        [AutoControlModelProperty(typeof(AddressControl))]
        public Address Address { get; set; }
        [AutoControlModelProperty(typeof(PersonControl))]
        public Person? Dad { get; set; }
        [AutoControlModelProperty(typeof(PersonControl))]
        public Person? Mom { get; set; }
    }

    public sealed class AddressAttributesProvider
    {
        private static readonly Dictionary<String, Object> _streetAttributes = new()
        {
            {"placeholder", nameof(Address.Street) }
        };
        private static readonly Dictionary<String, Object> _cityAttributes = new()
        {
            {"placeholder", nameof(Address.City) }
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
