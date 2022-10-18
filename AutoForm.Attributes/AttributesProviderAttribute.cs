using System;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes the target as the attributes provider for the models properties.
	/// <para>
	/// The value returned by this property must implement a method "Get-PropertyIdentifier-Attributes()" for every property of the model that is not excluded from control generation.
	/// </para>
	/// <para>
	/// Example: For a model that has the properties <c>Name</c> and <c>Age</c>, the attributes provider must implement the methods <c>GetNameAttributes</c> and <c>GetAgeAttributes</c>.
	/// </para>
	/// <para>
	/// The return type must be assignable to <see cref="IEnumerable{KeyValuePair{String, Object}}"/>.
	/// </para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	public sealed class AttributesProviderAttribute : Attribute
	{

	}
}