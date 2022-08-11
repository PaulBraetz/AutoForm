using System.Net.Http.Headers;
/// <summary>
/// Denotes the target property as the attributes provider for the models properties.
/// <para>
/// The value returned by this property must implement a method "Get-PropertyIdentifier-Attributes()" for every property of the model that is not excluded from control generation.
/// </para>
/// <para>
/// The return type must be assignable to <see cref="IEnumerable{KeyValuePair{String, Object}}()"/>.
/// </para>
/// <para>
/// Example: For a model that has a property .Name, the attributes provider must implement a method GetNameAttributes().
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class AutoControlAttributesProviderAttribute:Attribute
{

}