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

        private ModelExtractor Helper { get; set; }

        public void Execute(GeneratorExecutionContext context)
        {
            Helper = new ModelExtractor(context.Compilation);

            String source = String.Empty;
            var controlGenerator = GetControlGenerator();

            try
            {
                var compilation = context.Compilation;

                ModelSpace modelSpace = Helper.ExtractModelSpace();

                source = controlGenerator.Generate(modelSpace);
            }
            catch (Exception ex)
            {
                Error error = Helper.GetErrorModel(ex);

                source = controlGenerator.Generate(error);
            }
            finally
            {
                String filename = controlGenerator.Filename;

                context.AddSource(filename, source);
            }
        }
        public virtual void Initialize(GeneratorInitializationContext context)
        {

        }
    }
}
