using AutoForm.Generate.Blazor.Templates;
using AutoForm.Generate.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate.Blazor
{
    public sealed partial class BlazorSourceGenerator : IControlsSourceGenerator
    {
        public String Filename => "Controls";

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
