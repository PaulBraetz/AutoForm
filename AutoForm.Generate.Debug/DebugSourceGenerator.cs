using AutoForm.Generate.Models;
using System;

namespace AutoForm.Generate.Debug
{
	public sealed class DebugSourceGenerator : IControlsSourceGenerator
	{
		public String Filename => "AutoFormControls_Debug.g";


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
