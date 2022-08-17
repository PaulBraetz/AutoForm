using AutoForm.Analysis;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace AutoForm.Json.Analysis
{
    [Generator]
    public sealed class JsonGenerator : GeneratorBase
    {
        protected override IEnumerable<IControlsSourceGenerator> GetControlGenerators()
        {
            yield return new JsonSourceGenerator();
        }
    }
}
