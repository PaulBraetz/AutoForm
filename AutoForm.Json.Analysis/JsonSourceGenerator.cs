using AutoForm.Analysis;
using AutoForm.Analysis.Models;
using System;

namespace AutoForm.Json.Analysis
{
    internal sealed class JsonSourceGenerator : IControlsSourceGenerator
    {
        public String Filename => "ModelSpaceJson.g";


        public String Generate(ModelSpace modelSpace)
        {
            return $"//{{\"ModelSpace\":{modelSpace}}}";
        }

        public String Generate(Error error)
        {
            return $"//{{\"Error\":{error}}}";
        }
    }
}
