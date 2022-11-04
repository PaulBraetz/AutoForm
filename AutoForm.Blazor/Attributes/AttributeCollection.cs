using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AutoForm.Blazor.Attributes
{
	public readonly struct AttributeCollection : IEnumerable<KeyValuePair<String, Object>>
	{
		private readonly struct KeyValuePairComparer : IEqualityComparer<KeyValuePair<String, Object>>
		{
			public static readonly KeyValuePairComparer Instance = new();

			public Boolean Equals(KeyValuePair<String, Object> x, KeyValuePair<String, Object> y)
			{
				return x.Key == y.Key;
			}

			public Int32 GetHashCode([DisallowNull] KeyValuePair<String, Object> obj)
			{
				return obj.Key.GetHashCode();
			}
		}

		public static readonly AttributeCollection Empty = default;

		private readonly Dictionary<String, Object> _attributes;

		private static readonly IEnumerable<KeyValuePair<String, Object>> _emptyAttributes = Array.Empty<KeyValuePair<String, Object>>();
		public AttributeCollection(String key, Object value) : this((key, value))
		{
		}
		public AttributeCollection(params (String, Object)[] attributes) : this(attributes.Select(t => new KeyValuePair<String, Object>(t.Item1, t.Item2)))
		{
		}
		public AttributeCollection(IEnumerable<KeyValuePair<String, Object>> attributes)
		{
			_attributes = new Dictionary<string, object>(attributes.Distinct(KeyValuePairComparer.Instance));
		}
		/// <summary>
		/// Returns a new <see cref="AttributeCollection"/> the elements of which will be the union set of <paramref name="first"/> and <paramref name="second"/>.
		/// This means that there will be no duplicates in the resulting collection. An elements equality is determined by its key. Precedence will be given to 
		/// <paramref name="first"/>, meaning that if both <paramref name="first"/> and <paramref name="second"/> contain an element with the same key, the element 
		/// from <paramref name="first"/> will be included in the result set, while the element from <paramref name="second"/> will be excluded.
		/// </summary>
		/// <returns></returns>

		public static AttributeCollection Union(IEnumerable<KeyValuePair<String, Object>>? first, IEnumerable<KeyValuePair<String, Object>>? second)
		{
			return new AttributeCollection(first == null ? second ?? _emptyAttributes :
										   second == null ? first : first.Union(second, KeyValuePairComparer.Instance));
		}

		/// <summary>
		/// Returns a new <see cref="AttributeCollection"/> the elements of which will be the union set of <paramref name="attributes"/> and this collection.
		/// This means that there will be no duplicates in the resulting collection. An elements equality is determined by its key. Precedence will be given to 
		/// <paramref name="attributes"/>, meaning that if both <paramref name="attributes"/> and this collection contain an element with the same key, the element 
		/// from <paramref name="attributes"/> will be included in the result set, while the element from this collection will be excluded..
		/// </summary>
		/// <returns></returns>
		public AttributeCollection Union(IEnumerable<KeyValuePair<String, Object>> attributes)
		{
			return new AttributeCollection(Union(attributes, this._attributes));
		}

		public Boolean TryGetAttribute(String key, out Object? value)
		{
			return _attributes.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
		{
			return (_attributes ?? _emptyAttributes).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
