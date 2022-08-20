using AutoForm.Attributes;
using AutoForm.Blazor.Templates;
using System.Collections.ObjectModel;
using static TestApp.Models.AttributesFactory;

namespace TestApp.Models
{
    [Model]
    public sealed class Person
    {
        public Person()
        {
            Residency = new();
        }

        private String? name;
        private Byte age;

        [UseControl(typeof(AutoForm.Blazor.Controls.ByteRange))]
        [UseTemplate(typeof(AutoForm.Blazor.Templates.Empty))]
        public Byte Age { get => age; set => Console.WriteLine(age = value); }
        [UseControl(typeof(Controls.Progress))]
        [UseTemplate(typeof(Empty))]
        [Order(-1)]
        public Byte AgeDisplay { get => Age; set => Age = value; }
        public String? Name { get => name; set => Console.WriteLine(name = value); }
        public Address Residency { get; set; }

        [AttributesProvider]
        public PersonAttributesProvider AttributesProviderForPerson { get; } = new();
    }

    [Model]
    public sealed class Address
    {
        private String? street;
        private String? city;
        private Int16 houseNumber;

        public String? Street { get => street; set => Console.WriteLine(street = value); }
        public Int16 HouseNumber { get => houseNumber; set => Console.WriteLine(houseNumber = value); }
        public String? City { get => city; set => Console.WriteLine(city = value); }

        [AttributesProvider]
        public AddressAttributesProvider AttributesProvider { get; } = new();
    }

    public sealed class AddressAttributesProvider
    {
        private readonly IEnumerable<KeyValuePair<String, Object>> _houseNumber = AttributesFactory.Create(Options.Id | Options.FormControl, ("placeholder", nameof(Address.HouseNumber)));
        private readonly IEnumerable<KeyValuePair<String, Object>> _street = AttributesFactory.Create(Options.Id | Options.FormControl, ("placeholder", nameof(Address.Street)));
        private readonly IEnumerable<KeyValuePair<String, Object>> _city = AttributesFactory.Create(Options.Id | Options.FormControl, ("placeholder", nameof(Address.City)));

        public IEnumerable<KeyValuePair<String, Object>> GetHouseNumberAttributes()
        {
            return _houseNumber;
        }

        public IEnumerable<KeyValuePair<String, Object>> GetStreetAttributes()
        {
            return _street;
        }

        public IEnumerable<KeyValuePair<String, Object>> GetCityAttributes()
        {
            return _city;
        }
    }

    public sealed class PersonAttributesProvider
    {
        private readonly ReadOnlyDictionary<String, Object> _empty = new(new Dictionary<String, Object>());
        private readonly IEnumerable<KeyValuePair<String, Object>> _residency = AttributesFactory.Create(Options.Id, ("label", nameof(Person.Residency)));
        private readonly IEnumerable<KeyValuePair<String, Object>> _name = AttributesFactory.Create(Options.Id | Options.FormControl, ("placeholder", nameof(Person.Name)));
        private readonly IEnumerable<KeyValuePair<String, Object>> _age = AttributesFactory.Create(Options.Id | Options.FormControl, ("label", nameof(Person.Age)), ("class", "form-range"));

        public IEnumerable<KeyValuePair<String, Object>> GetResidencyAttributes()
        {
            return _residency;
        }

        public IEnumerable<KeyValuePair<String, Object>> GetNameAttributes()
        {
            return _name;
        }

        public IEnumerable<KeyValuePair<String, Object>> GetAgeAttributes()
        {
            return _age;
        }

        public IEnumerable<KeyValuePair<String, Object>> GetAgeDisplayAttributes()
        {
            return _empty;
        }
    }

    internal static class AttributesFactory
    {
        [Flags]
        public enum Options
        {
            None = 0,
            Id = 1,
            FormControl = 2
        }
        public static IEnumerable<KeyValuePair<String, Object>> Create(params (String, Object)[] attributes)
        {
            return Create(Options.Id, attributes);
        }
        public static IEnumerable<KeyValuePair<String, Object>> Create(Options options, params (String, Object)[] attributes)
        {
            var result = new Dictionary<String, Object>();

            foreach (var attribute in attributes)
            {
                if (!result.ContainsKey(attribute.Item1))
                {
                    result.Add(attribute.Item1, attribute.Item2);
                }
            }

            if (options.HasFlag(Options.Id) && !result.ContainsKey("id"))
            {
                result.Add("id", $"component_{Guid.NewGuid()}");
            }

            if (options.HasFlag(Options.FormControl) && !result.ContainsKey("class"))
            {
                result.Add("class", "form-control");
            }

            return result;
        }
    }
}
