using AutoForm.Analysis.Models;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis
{
    public abstract class GeneratorBase : ISourceGenerator
    {
        protected abstract IEnumerable<IControlsSourceGenerator> GetControlGenerators();

        private ModelExtractor Helper { get; set; }

        public void Execute(GeneratorExecutionContext context)
        {
            Helper = new ModelExtractor(context.Compilation);

            var source = String.Empty;
            var controlGenerators = GetControlGenerators();

            var sourceNames = controlGenerators.Select(g => g.Filename);
            var duplicates = new List<String>();
            foreach (var sourceName in sourceNames)
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

            foreach (var controlGenerator in controlGenerators)
            {
                try
                {
                    var compilation = context.Compilation;

                    var modelSpace = Helper.ExtractModelSpace();

                    source = controlGenerator.Generate(modelSpace);
                }
                catch (Exception ex)
                {
                    var error = Helper.GetErrorModel(ex);

                    source = controlGenerator.Generate(error);
                }
                finally
                {
                    var filename = controlGenerator.Filename;

                    context.AddSource(filename, source);
                }
            }
        }
        public virtual void Initialize(GeneratorInitializationContext context)
        {

        }
    }
}
