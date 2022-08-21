namespace TestApp.Models
{
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
