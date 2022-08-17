using AutoForm.Analysis;
using AutoForm.Analysis.Models;
using AutoForm.Generate.Blazor.Templates;
using System;

namespace AutoForm.Blazor.Analysis
{
    public sealed class BlazorSourceGenerator : IControlsSourceGenerator
    {
        public String Filename => "GeneratedControls.g";

        public String Generate(ModelSpace modelSpace)
        {
            return SourceFactory.Create(modelSpace).Build();
        }

        public String Generate(Error error)
        {
            return SourceFactory.Create(error).Build();
        }
    }
}
