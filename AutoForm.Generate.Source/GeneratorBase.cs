using AutoForm.Generate.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoForm.Generate
{
	public abstract class GeneratorBase : ISourceGenerator
	{
		protected abstract IControlsSourceGenerator GetControlGenerator();

		protected ModelHelper Helper { get; private set; }

		public void Execute(GeneratorExecutionContext context)
		{
			Helper = new ModelHelper(context.Compilation);

			String source = String.Empty;
			var controlGenerator = GetControlGenerator();
			try
			{
				var compilation = context.Compilation;

				ModelSpace modelSpace = Helper.GetModelSpace();

				source = controlGenerator.Generate(modelSpace);
			}
			catch (Exception ex)
			{
				Error error = Helper.GetError(ex);

				source = controlGenerator.Generate(error);
			}
			finally
			{
				context.AddSource($"Controls.g", source);
			}
		}
		public virtual void Initialize(GeneratorInitializationContext context)
		{

		}
	}
}
