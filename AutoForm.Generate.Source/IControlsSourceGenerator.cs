using AutoForm.Generate.Models;
using System;

namespace AutoForm.Generate
{
	public interface IControlsSourceGenerator
	{
		String Filename { get; }
		String Generate(ModelSpace modelSpace);
		String Generate(Error error);
	}
}