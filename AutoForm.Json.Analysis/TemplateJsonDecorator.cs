using AutoForm.Analysis;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace AutoForm.Json.Analysis
{
	internal readonly struct TemplateJsonDecorator : IJsonDecorator<Template>, IEquatable<IJson>
	{
		public Template Value { get; }

		public TemplateJsonDecorator(Template value) : this()
		{
			Value = value;
			_json = Analysis.
		}

		private readonly String _json;
		public String Json => _json ?? "null";
		public override string ToString()
		{
			return Json;
		}

		public override Boolean Equals(Object obj)
		{
			return obj is IJson json && Equals(json);
		}

		public Boolean Equals(IJson other)
		{
			return Json == other.Json;
		}

		public override Int32 GetHashCode()
		{
			return 885466328 + Json.GetHashCode();
		}

		public static Boolean operator ==(TemplateJsonDecorator left, TemplateJsonDecorator right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(TemplateJsonDecorator left, TemplateJsonDecorator right)
		{
			return !(left == right);
		}
	}
}
