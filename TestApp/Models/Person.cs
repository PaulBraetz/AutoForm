using AutoForm.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestApp.Models
{

    [AutoControlModel]
    public sealed class Person
    {
        [AutoControlModel]
        public sealed class Address
        {
            private String? street;

            [MaxLength(10)]
            public String? Street { get => street; set => Console.WriteLine(street = value); }

            [MaxLength(10)]
            [AutoControlPropertyOrder(-1)]
            public String? City { get; set; }

            [AutoControlAttributesProvider]
            public AddressAttributesProvider AttributesProvider { get; } = new();
        }
        public Person()
        {
            Location1 = new();
            Location2 = new();
        }
        [AutoControlPropertyOrder(Int32.MaxValue)]
        public Address Location1 { get; set; }

        [AutoControlPropertyOrder(0)]
        public Address Location2 { get; set; }

        [AutoControlAttributesProvider]
        public PersonAttributesProvider AttributesProvider { get; } = new();
    }
    public sealed class AddressAttributesProvider
    {
        private readonly IEnumerable<KeyValuePair<String, Object>> _default = new Dictionary<String, Object>(){
                    {"class", "form-control" }
                };

        public IEnumerable<KeyValuePair<String, Object>> GetStreetAttributes() => _default;
        public IEnumerable<KeyValuePair<String, Object>> GetCityAttributes() => _default;
    }
    public sealed class PersonAttributesProvider
    {
        private readonly IEnumerable<KeyValuePair<String, Object>> _location1 = new Dictionary<String, Object>(){
                    {"label", nameof(Person.Location1)}
                };

        private readonly IEnumerable<KeyValuePair<String, Object>> _location2 = new Dictionary<String, Object>(){
                    {"label", nameof(Person.Location2)}
                };

        public IEnumerable<KeyValuePair<String, Object>> GetLocation1Attributes() => _location1;
        public IEnumerable<KeyValuePair<String, Object>> GetLocation2Attributes() => _location2;
    }
}
