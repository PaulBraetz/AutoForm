using AutoForm.Generate.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate
{
    public abstract class GeneratorBase : ISourceGenerator
    {
        protected abstract IEnumerable<IControlsSourceGenerator> GetControlGenerators();

        private ModelExtractor Helper { get; set; }

        public void Execute(GeneratorExecutionContext context)
        {
            Helper = new ModelExtractor(context.Compilation);

            String source = String.Empty;
            IEnumerable<IControlsSourceGenerator> controlGenerators = GetControlGenerators();

            var sourceNames = controlGenerators.Select(g => g.Filename);
            var duplicates = new List<String>();
            foreach (String sourceName in sourceNames)
            {
                if (sourceNames.Count(s => s == sourceName) > 1)
                {
                    duplicates.Add(sourceName);
                }
            }
            if (duplicates.Any())
            {
                throw new Exception($"Duplicate source filenames have been registered: {String.Join(", ", duplicates)}");
            }


            foreach (IControlsSourceGenerator controlGenerator in controlGenerators)
            {
                try
                {
                    Compilation compilation = context.Compilation;

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
        }
        public virtual void Initialize(GeneratorInitializationContext context)
        {

        }
    }
}
