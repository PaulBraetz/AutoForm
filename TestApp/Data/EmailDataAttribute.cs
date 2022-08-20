using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TestApp.Data
{
	public class EmailDataAttribute : ValidationAttribute
	{
		public EmailDataAttribute()
		{
		}

		public EmailDataAttribute(Func<String> errorMessageAccessor) : base(errorMessageAccessor)
		{
		}

		public EmailDataAttribute(String errorMessage) : base(errorMessage)
		{
		}

		public override Boolean RequiresValidationContext => false;
		public override Boolean IsValid(Object? value)
		{
			return value is String stringValue && Regex.IsMatch(stringValue, @"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");
		}
	}
}
