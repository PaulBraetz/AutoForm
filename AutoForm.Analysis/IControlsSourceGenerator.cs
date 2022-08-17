using AutoForm.Analysis.Models;
using System;

namespace AutoForm.Analysis
{
    public interface IControlsSourceGenerator
    {
        String Filename { get; }
        String Generate(ModelSpace modelSpace);
        String Generate(Error error);
    }
}