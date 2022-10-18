using AutoForm.Json.Analysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Xml.Linq;

namespace Test
{
	[TestClass]
	public class JsonDecoratorTests
	{
		private record Address(String? Street, Int32 HouseNumber) { }
		private record Person(String? Name, Int32 Age, Address?[] Adresses) { }

		private static readonly IEnumerable<Person?> _people = new Person?[]
		{
			null,
			new Person("Jon", 32, new[]
			{
				new Address("South St",14),
				new Address(null, 10),
				new Address("West Ave", 21),
				null
			}),
			new Person("Jake", 5, new[]
			{
				new Address("West Ave",20),
				new Address("South St", -23),
				new Address("Wall St", 20)
			}),
			new Person("Mary", 28, new[]
			{
				new Address(null,18),
				new Address("Hellbound Ave", 15),
				null,
				new Address("Jackson St", -4)
			}),

		}.ToList().AsReadOnly();

		[TestMethod]
		public void TestNull()
		{
			var expected = "null";
			var actual = JsonDecorator<Object>.Null().Json;
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestString()
		{
			foreach (var person in _people)
			{
				var name = person?.Name;
				var expected = name != null ?
					$"\"{name}\"" :
					"null";
				var actual = JsonDecorator<String?>.String(name).Json;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void TestStringArray()
		{
			var names = _people.Select(p => p?.Name).ToArray();
			var expected = $"[{String.Join(",", names.Select(s => JsonDecorator<String?>.String(s).Json))}]";
			var actual = JsonDecorator<String?>.StringArray(names).Json;
		}

		[TestMethod]
		public void TestNumber()
		{
			foreach (var person in _people)
			{
				var age = person?.Age;
				var expected = age?.ToString() ?? "null";
				var actual = JsonDecorator<Int32?>.Number(age).Json;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void TestNumberArray()
		{
			var ages = _people.Select(p => p?.Age).ToArray();
			var expected = $"[{String.Join(",", ages.Select(a => JsonDecorator<Int32?>.Number(a).Json))}]";
			var actual = JsonDecorator<Int32?>.NumberArray(ages).Json;
		}

		[TestMethod]
		public void TestToString()
		{
			foreach (var person in _people)
			{
				var name = person?.Name;
				var decorator = JsonDecorator<String?>.String(name);
				var expected = decorator.Json;
				var actual = decorator.ToString();
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void TestOriginalValue()
		{
			foreach (var person in _people)
			{
				var expected = person?.Name;
				var actual = JsonDecorator<String?>.String(expected).OriginalValue;
				Assert.AreEqual(expected, actual);
			}

			foreach (var person in _people)
			{
				var expected = person?.Age;
				var actual = JsonDecorator<Int32?>.String(expected).OriginalValue;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void TestKeyValuePair()
		{
			foreach (var person in _people)
			{
				var name = person?.Name;
				var expected = $"{JsonDecorator<String>.String(nameof(Person.Name))}:{JsonDecorator<String?>.String(name)}";
				var actual = JsonDecorator<String?>.KeyValuePair(nameof(Person.Name), JsonDecorator<String?>.String(name)).Json;
				Assert.AreEqual(expected, actual);
			}
		}

		private static IJson[] AddressJsonMemberFactory(Address address)
		{
			return new IJson[]
			{
				JsonDecorator<String?>.KeyValuePair(nameof(Address.Street), JsonDecorator<String?>.String(address.Street)),
				JsonDecorator<Int32?>.KeyValuePair(nameof(Address.HouseNumber), JsonDecorator<Int32?>.Number(address.HouseNumber))
			};
		}

		private static IJson[] PersonJsonMemberFactory(Person person)
		{
			return new IJson[]
			{
				JsonDecorator<String?>.KeyValuePair(nameof(Person.Name), JsonDecorator<String?>.String(person.Name)),
				JsonDecorator<Int32>.KeyValuePair(nameof(Person.Age), JsonDecorator<Int32>.Number(person.Age)),
				JsonDecorator<Address?[]>.KeyValuePair(nameof(Person.Adresses),person.Adresses!=null?JsonDecorator<Address?>.ObjectArray(person.Adresses, AddressJsonMemberFactory!):JsonDecorator<Address?[]>.Null()!)
			};
		}

		[TestMethod]
		public void TestObject()
		{
			foreach (var address in _people.SelectMany(p => p?.Adresses ?? Array.Empty<Address>()))
			{
				var expected = address != null ?
					$"{{{String.Join(",", AddressJsonMemberFactory(address).Select(j => j.Json))}}}" :
					"null";
				var actual = JsonDecorator<Address?>.Object(address, AddressJsonMemberFactory!).Json;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void TestObjectArray()
		{
			foreach (var person in _people)
			{
				var addresses = person?.Adresses ?? Array.Empty<Address>();
				var expected = $"[{String.Join(",", addresses.Select(a => JsonDecorator<Address?>.Object(a, AddressJsonMemberFactory!).Json))}]";
				var actual = JsonDecorator<Address?>.ObjectArray(addresses, AddressJsonMemberFactory!).Json;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void TestComplexObject()
		{
			foreach (var person in _people)
			{
				var expected = person != null ?
					$"{{{String.Join(",", PersonJsonMemberFactory(person).Select(j => j.Json))}}}" :
					"null";
				var actual = JsonDecorator<Person?>.Object(person, PersonJsonMemberFactory!).Json;
				Assert.AreEqual(expected, actual);
			}
		}

	}
}