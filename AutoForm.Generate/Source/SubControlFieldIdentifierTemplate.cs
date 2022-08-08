using System;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		private readonly struct SubControlFieldIdentifierTemplate
		{
			private const String TEMPLATE = "_subControlType_" + SUB_CONTROL_TYPE_FIELD_INDEX;

			public String Build(ref Int32 subControlTypeFieldIndex)
			{
				var result = TEMPLATE
					.Replace(SUB_CONTROL_TYPE_FIELD_INDEX, subControlTypeFieldIndex.ToString());
				subControlTypeFieldIndex++;
				return result;
			}
		}
	}

}
