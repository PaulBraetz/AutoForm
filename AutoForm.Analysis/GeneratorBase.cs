﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace AutoForm.Analysis
{
	internal abstract class GeneratorBase : ISourceGenerator
	{
		protected abstract void OnModelSpaceCreated(GeneratorExecutionContext context, ModelSpace modelSpace);
		protected abstract void OnError(GeneratorExecutionContext context, Error error);

		public void Execute(GeneratorExecutionContext context)
		{
			if (context.SyntaxContextReceiver is IModelExtractorData data)
			{
				try
				{
					var extractor = new ModelExtractor(context.Compilation, data);
					var modelSpace = extractor.ExtractModelSpace();
					OnModelSpaceCreated(context, modelSpace);
				}
				catch (Exception ex)
				{
					var error = Error.Create().WithFlattened(ex);
					OnError(context, error);
					throw;
				}
			}
		}
		public virtual void Initialize(GeneratorInitializationContext context)
		{
			context.RegisterForSyntaxNotifications(() => new SyntaxContextReceiver());
		}

		private sealed class SyntaxContextReceiver : ISyntaxContextReceiver, IModelExtractorData
		{
			private HashSet<BaseTypeDeclarationSyntax> Models { get; } = new HashSet<BaseTypeDeclarationSyntax>();
			IEnumerable<BaseTypeDeclarationSyntax> IModelExtractorData.Models => Models;
			private HashSet<BaseTypeDeclarationSyntax> FallbackControls { get; } = new HashSet<BaseTypeDeclarationSyntax>();
			IEnumerable<BaseTypeDeclarationSyntax> IModelExtractorData.FallbackControls => FallbackControls;
			private HashSet<BaseTypeDeclarationSyntax> FallbackTemplates { get; } = new HashSet<BaseTypeDeclarationSyntax>();
			IEnumerable<BaseTypeDeclarationSyntax> IModelExtractorData.FallbackTemplates => FallbackTemplates;
			private HashSet<BaseTypeDeclarationSyntax> UseControls { get; } = new HashSet<BaseTypeDeclarationSyntax>();
			IEnumerable<BaseTypeDeclarationSyntax> IModelExtractorData.UseControls => UseControls;
			private HashSet<BaseTypeDeclarationSyntax> UseTemplates { get; } = new HashSet<BaseTypeDeclarationSyntax>();
			IEnumerable<BaseTypeDeclarationSyntax> IModelExtractorData.UseTemplates => UseTemplates;

			public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
			{
				if (context.Node is BaseTypeDeclarationSyntax typeDeclaration)
				{
					var semanticModel = context.SemanticModel;
					if (typeDeclaration.AttributeLists.HasAttributes(semanticModel, Attributes.UseControl))
					{
						UseControls.Add(typeDeclaration);
					}

					if (typeDeclaration.AttributeLists.HasAttributes(semanticModel, Attributes.FallbackControl))
					{
						FallbackControls.Add(typeDeclaration);
					}

					if (typeDeclaration.AttributeLists.HasAttributes(semanticModel, Attributes.UseTemplate))
					{
						UseTemplates.Add(typeDeclaration);
					}

					if (typeDeclaration.AttributeLists.HasAttributes(semanticModel, Attributes.FallbackTemplate))
					{
						FallbackTemplates.Add(typeDeclaration);
					}
				}
				else if (context.Node is PropertyDeclarationSyntax propertyDeclaration &&
						propertyDeclaration.AttributeLists.HasAttributes(
							context.SemanticModel,
							Attributes.AttributesProvider,
							Attributes.ModelProperty))
				{
					SyntaxNode parent = propertyDeclaration;
					while (!(parent is BaseTypeDeclarationSyntax))
					{
						parent = parent.Parent;
					}

					Models.Add((BaseTypeDeclarationSyntax)parent);
				}
			}
		}
	}
}
