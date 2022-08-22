namespace TestApp.AttributesProviders
{
	public class MyAttributesProvider
	{
		public IEnumerable<KeyValuePair<String, Object>> GetNameAttributes()
		{
			return new Dictionary<String, Object>()
			{
				{"placeholder", "Name" },
				{"class", "form-control" },
				{"label", "User Name" }
			};
		}
		public IEnumerable<KeyValuePair<String, Object>> GetAgeAttributes()
		{
			return new Dictionary<String, Object>()
			{
				{"min", "0" },
                {"class", "form-control" },
				{"label", "User Age" }
            };
        }
    }
}
