using AutoForm.Attributes;
using Microsoft.AspNetCore.Components;
using TestApp.Models;

namespace TestApp.Templates
{
    [FallbackTemplate(typeof(Address))]
    public partial class AddressControlTemplate
    {
    }
}
