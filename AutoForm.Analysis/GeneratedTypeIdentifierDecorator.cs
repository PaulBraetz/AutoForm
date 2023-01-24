using System;
using System.Collections.Generic;

using RhoMicro.CodeAnalysis;

namespace AutoForm.Analysis
{
	internal readonly struct GeneratedTypeIdentifierDecorator : ITypeIdentifier, IEquatable<GeneratedTypeIdentifierDecorator>, IEquatable<ITypeIdentifier>
	{
		public GeneratedTypeIdentifierDecorator(ITypeIdentifier decoree) => _decoree = decoree;

		private readonly ITypeIdentifier _decoree;

		public ITypeIdentifierName Name => _decoree.Name;

		public INamespace Namespace => _decoree.Namespace;

		public override Boolean Equals(Object obj)
		{
			return obj is GeneratedTypeIdentifierDecorator decorator && Equals(decorator) ||
				   obj is ITypeIdentifier identifier && Equals(identifier);
		}

		public Boolean Equals(GeneratedTypeIdentifierDecorator other) => EqualityComparer<ITypeIdentifier>.Default.Equals(_decoree, other._decoree);
		public Boolean Equals(ITypeIdentifier other) => EqualityComparer<ITypeIdentifier>.Default.Equals(_decoree, other);

		public override Int32 GetHashCode() => -1320898091 + EqualityComparer<ITypeIdentifier>.Default.GetHashCode(_decoree);

		public static Boolean operator ==(GeneratedTypeIdentifierDecorator left, GeneratedTypeIdentifierDecorator right) => left.Equals(right);

		public static Boolean operator !=(GeneratedTypeIdentifierDecorator left, GeneratedTypeIdentifierDecorator right) => !(left == right);

		public override String ToString() => _decoree.ToString();
	}
}
