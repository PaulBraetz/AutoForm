namespace TestApp.AttributesProviders
{
	public class MyAttributesProvider
	{
		public IEnumerable<KeyValuePair<String, Object>> GetNameAttributes()
		{
			return new Dictionary<String, Object>()
			{
				{"placeholder", "Name" }
			};
		}
		public IEnumerable<KeyValuePair<String, Object>> GetAgeAttributes()
		{
			return new Dictionary<String, Object>()
			{
				{"min", "0" }
			};
		}
	}
}
