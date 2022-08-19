using AutoForm.Attributes;
using AutoForm.Blazor;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
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

		private String name;

		public String Name { get => name; set => Console.WriteLine(name = value); }
		public Address Residency { get; set; }

		[AttributesProvider]
		public PersonAttributesProvider AttributesProvider { get; } = new();
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
		private readonly IDictionary<String, Object> _houseNumber = AttributesFactory.Create(Options.Id | Options.FormControl, ("placeholder", nameof(Address.HouseNumber)));
		private readonly IDictionary<String, Object> _street = AttributesFactory.Create(Options.Id | Options.FormControl, ("placeholder", nameof(Address.Street)));
		private readonly IDictionary<String, Object> _city = AttributesFactory.Create(Options.Id | Options.FormControl, ("placeholder", nameof(Address.City)));

		public IDictionary<String, Object> GetHouseNumberAttributes() => _houseNumber;
		public IDictionary<String, Object> GetStreetAttributes() => _street;
		public IDictionary<String, Object> GetCityAttributes() => _city;
	}

	public sealed class PersonAttributesProvider
	{
		private readonly IDictionary<String, Object> _residency = AttributesFactory.Create(Options.Id, ("label", nameof(Person.Residency)));
		private readonly IDictionary<String, Object> _name = AttributesFactory.Create(Options.Id | Options.FormControl, ("placeholder", nameof(Person.Name)));

		public IDictionary<String, Object> GetResidencyAttributes() => _residency;
		public IDictionary<String, Object> GetNameAttributes() => _name;
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
		public static IDictionary<String, Object> Create(params (String, Object)[] attributes)
		{
			return Create(Options.Id, attributes);
		}
		public static IDictionary<String, Object> Create(Options options, params (String, Object)[] attributes)
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
