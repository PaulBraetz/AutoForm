using System;

namespace AutoForm.Generate
{
    internal static partial class Source
    {
        private readonly struct ControlTypeIdentifierTemplate
        {
            private const String TEMPLATE = "__Control_" + CONTROL_INDEX;

            public String Build(ref Int32 controlIndex)
            {
                var result = TEMPLATE
                    .Replace(CONTROL_INDEX, controlIndex.ToString());
                controlIndex++;
                return result;
            }

            public override String ToString()
            {
                Int32 controlIndex = 0;
                return Build(ref controlIndex);
            }
        }
    }
}
