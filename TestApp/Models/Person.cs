using AutoForm.Attributes;
using AutoForm.Blazor;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestApp.Models
{
	[Model]
	public sealed class Person
	{
		public Person()
		{
			Residency = new();
		}
		public Address Residency { get; set; }

		[AttributesProvider]
		public PersonAttributesProvider AttributesProvider { get; } = new();
	}

	[Model]
	public sealed class Address
	{
		private String? street;
		[Order(-2)]
		public String? Street { get => street; set => Console.WriteLine($"{street = value}, {HouseNumber}, {City}"); }
		public String? City { get; set; }
		[Order(-1)]
		public Int16 HouseNumber { get; set; }

		[AttributesProvider]
		public AddressAttributesProvider AttributesProvider { get; } = new();
	}

	public sealed class AddressAttributesProvider
	{
		private readonly IDictionary<String, Object> _houseNumber =
			AttributesFactory.Create(("class", "form-control"), ("placeholder", nameof(Address.HouseNumber)));
		private readonly IDictionary<String, Object> _street =
			AttributesFactory.Create(("class", "form-control"), ("placeholder", nameof(Address.Street)));
		private readonly IDictionary<String, Object> _city =
			AttributesFactory.Create(("class", "form-control"), ("placeholder", nameof(Address.City)));

		public IDictionary<String, Object> GetHouseNumberAttributes() => _houseNumber;
		public IDictionary<String, Object> GetStreetAttributes() => _street;
		public IDictionary<String, Object> GetCityAttributes() => _city;
	}

	public sealed class PersonAttributesProvider
	{
		private readonly IDictionary<String, Object> _residency = AttributesFactory.Create(("label", nameof(Person.Residency)));

		public IDictionary<String, Object> GetResidencyAttributes() => _residency;
	}

	internal static class AttributesFactory
	{
		public static IDictionary<String, Object> Create(params (String, Object)[] attributes)
		{
			return Create(true, attributes);
		}
		public static IDictionary<String, Object> Create(Boolean appendId, params (String, Object)[] attributes)
		{
			var result = new Dictionary<String, Object>();

			foreach (var attribute in attributes)
			{
				if (!result.ContainsKey(attribute.Item1))
				{
					result.Add(attribute.Item1, attribute.Item2);
				}
			}

			if (appendId && !result.ContainsKey("id"))
			{
				result.Add("id", $"component_{Guid.NewGuid()}");
			}

			return result;
		}
	}
}
