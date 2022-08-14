using AutoForm.Generate.Models;
using System;

namespace AutoForm.Generate
{
	public interface IControlsSourceGenerator
	{
		String Generate(ModelSpace modelSpace);
		String Generate(Error error);
	}
}
