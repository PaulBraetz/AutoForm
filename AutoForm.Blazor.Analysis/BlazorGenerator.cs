using AutoForm.Analysis;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace AutoForm.Blazor.Analysis
{
    [Generator]
    public sealed class BlazorGenerator : GeneratorBase
    {
        protected override IEnumerable<IControlsSourceGenerator> GetControlGenerators()
        {
            return new[] { new BlazorSourceGenerator() };
        }
    }
}
