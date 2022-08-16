using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace AutoForm.Generate.Blazor
{
    [Generator]
    public sealed class Generator : GeneratorBase
    {
        protected override IEnumerable<IControlsSourceGenerator> GetControlGenerators()
        {
            return new[] { new BlazorSourceGenerator() };
        }
    }
}
