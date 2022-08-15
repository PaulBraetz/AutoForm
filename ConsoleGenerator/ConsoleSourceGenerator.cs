using AutoForm.Generate;
using AutoForm.Generate.Models;
using System;

namespace ConsoleGenerator
{
    internal readonly struct DirectSourceGenerator : IControlsSourceGenerator
    {
        public String Filename => "Debug.g";


        public String Generate(ModelSpace modelSpace)
        {
            return $"{{\"ModelSpace\":{modelSpace}}}";
        }

        public String Generate(Error error)
        {
            return $"{{\"Error\":{error}}}";
        }
    }
}
