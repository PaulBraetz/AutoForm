# AutoForm #

AutoForm is a model-driven UI control generation tool for .Net. Using attributes and roslyn code generators, UI design for controls can be largely automated.

## Features ##

* Attribute-based model discovery
* Debug json generator
* Blazor component generator

## Versioning ##

AutoForm uses [Semantic Versioning 2.0.0](https://semver.org/).

## Installation ##

Currently, there are multiple packages published for generating code based on models:

* For model discovery, an attribute collection is provided in the AutoForm.Attributes package.
* For debug purposes, a json generator is provided in the AutoForm.Json.Analysis package.
* For use in Blazor, a blazor component generator is provided in the AutoForm.Blazor.Analysis package.
* For accessing controls generated by AutoForm.Blazor.Analysis, an entry point control and other default controls and templates are provided in the AutoForm.Blazor package.

In order to use the generators, models have to be annotated using the attributes found in AutoForm.Attributes. Since the generators rely on code semantics in order to discover models, you may implement your own attributes using the same namespace and attribute names as in AutoForm.Attributes. This is currently not reccomended, as the underlying processes are not hardened against faulty attribute declarations.

### Installing AutoForm.Attributes ###

Nuget Gallery: https://www.nuget.org/packages/RhoMicro.AutoForm.Attributes

Package Manager: `Install-Package RhoMicro.AutoForm.Attributes -Version 1.0.2`

.Net CLI: `dotnet add package RhoMicro.AutoForm.Attributes --version 1.0.2`

### Installing the json generator ###

Nuget Gallery: https://www.nuget.org/packages/RhoMicro.AutoForm.Json.Analyzer

Package Manager: `Install-Package RhoMicro.AutoForm.Json.Analyzer -Version 1.0.1`

.Net CLI: `dotnet add package RhoMicro.AutoForm.Json.Analyzer --version 1.0.1`

### Installing the blazor generator ###

Nuget Gallery: https://www.nuget.org/packages/RhoMicro.AutoForm.Blazor.Analyzer

Package Manager: `Install-Package RhoMicro.AutoForm.Blazor.Analyzer -Version 1.0.1`

.Net CLI: `dotnet add package RhoMicro.AutoForm.Blazor.Analyzer --version 1.0.1`

### Installing the blazor entry point ###

Nuget Gallery: https://www.nuget.org/packages/RhoMicro.AutoForm.Blazor

Package Manager: `Install-Package RhoMicro.AutoForm.Blazor -Version 1.0.1`

.Net CLI: `dotnet add package RhoMicro.AutoForm.Blazor --version 1.0.1`

## How To Use ##

Annotate your model with `AutoForm.Attributes.ModelAttribute` :
```cs
[Model]
public class Model
{
  public Byte Age { get; set; }
  public String? Name { get; set; }
}
```
*Found here: [TestApp.Models.MyModel](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Models/MyModel.cs)*

The control generated for this model will look something like this:
```cs
private sealed class __Control_TestApp_Models_MyModel : ControlBase<TestApp.Models.MyModel>
{
  protected override void BuildRenderTree(RenderTreeBuilder __builder)
  {
    if(Value != null)
    {
      //Subcontrol for Age
      __builder.OpenComponent<AutoForm.Blazor.Controls.ByteNumber>(0);
      __builder.AddAttribute(1, "Value", RuntimeHelpers.TypeCheck<Byte>(Value.Age));
      __builder.AddAttribute(2, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<Byte>>(EventCallback.Factory.Create<Byte>(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value.Age = __value; ValueChanged.InvokeAsync(Value);}, Value.Age))));
      __builder.CloseComponent();

      //Subcontrol for Name
      __builder.OpenComponent<AutoForm.Blazor.Controls.Text>(3);
      __builder.AddAttribute(4, "Value", RuntimeHelpers.TypeCheck<String>(Value.Name));
      __builder.AddAttribute(5, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<String>>(EventCallback.Factory.Create<String>(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value.Name = __value; ValueChanged.InvokeAsync(Value);}, Value.Name))));
      __builder.CloseComponent();
    }
  }
}
```

Access the generated control using an `AutoForm.Blazor.AutoControl<MyModel>` :
```razor
@using AutoForm.Blazor`

<AutoControl @bind-Value="Model" />

@code {
  MyModel Model { get; set; } = new MyModel();
}
```
*Found here: [TestApp.Pages.Index](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Pages/Index.razor)*

Note that default controls provided by AutoForm.Blazor, found in `AutoForm.Blazor.Controls`, will bind to the `oninput` event. This means that when using these controls, models will be updated with every keystroke instead of when a control loses focus, as is standard.

The html rendered will look something like this:

![Image Missing](https://static.rhomicro.com/files/images/github/autoform/1.png)

A template may be used in order surround the control with another component. For additional attributes on the control itself, an attributes provider can be used.

#### Using a template ####

Templates must implement the following properties:
```cs
[Parameter]
public TModel Value { get; set; }

[Parameter]
public IEnumerable<KeyValuePair<String, Object>> Attributes { get; set; }

[Parameter]
public RenderFragment? ChildContent { get; set; }
```
where TModel is the model type whose control this template should be applied to. These properties enable the template to access the controls attributes as well as the models current value. The control will be passed to `ChildContent`.
These properties are implemented in `AutoForm.Blazor.Templates.Abstractions.TemplateBase`.

Create a template like so:
```razor
@inherits AutoForm.Blazor.Templates.Abstractions.TemplateBase<String>

<div class="input-group mb-3">
  <span class="input-group-text" >Enter Value:</span>
  @ChildContent
</div>
```
*Found here: [TestApp.Templates.MyTemplate](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Templates/MyTemplate.razor)*

Use the template like so:
```cs
[Model]
public class Model
{
  public Byte Age { get; set; }
  
  [AutoForm.Attributes.UseTemplate(typeof(Templates.MyTemplate))]
  public String? Name { get; set; }
}
```
*Found here: [TestApp.Models.MyModel](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Models/MyModel.cs)*

The control generated for this model will now look something like this:
```cs
private sealed class __Control_TestApp_Models_MyModel : ControlBase<TestApp.Models.MyModel>
{
  protected override void BuildRenderTree(RenderTreeBuilder __builder)
  {
    if(Value != null)
    {
      //Subcontrol for Age
      __builder.OpenComponent<AutoForm.Blazor.Controls.ByteNumber>(0);
      __builder.AddAttribute(1, "Value", RuntimeHelpers.TypeCheck<Byte>(Value.Age));
      __builder.AddAttribute(2, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<Byte>>(EventCallback.Factory.Create<Byte>(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value.Age = __value; ValueChanged.InvokeAsync(Value);}, Value.Age))));
      __builder.CloseComponent();

      //Template for Name
      __builder.OpenComponent<Templates.MyTemplate>(3);
      __builder.AddAttribute(4, "Value", RuntimeHelpers.TypeCheck<System.String>(Value.Name));
      __builder.AddAttribute(5, "ChildContent", (RenderFragment)(buildNameSubControl));
      __builder.CloseComponent();

      void buildNameSubControl(RenderTreeBuilder __builder)
      {
        //Subcontrol for Name
        __builder.OpenComponent<AutoForm.Blazor.Controls.Text>(6);
        __builder.AddAttribute(7, "Value", RuntimeHelpers.TypeCheck<System.String>(Value.Name));
        __builder.AddAttribute(8, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<String>>(EventCallback.Factory.Create<System.String>(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value.Name = __value; ValueChanged.InvokeAsync(Value);}, Value.Name))));
        __builder.CloseComponent();
      }
    }
  }
}
```

The html rendered will now look something like this:

![Image Missing](https://static.rhomicro.com/files/images/github/autoform/2.png)

#### Using an attribute provider ####

Attribute providers must implement a method "Get-PropertyIdentifier-Attributes()" for every property of the model that is not excluded from control generation. The return value must be assignable to `IEnumerable<KeyValuePair<String, Object>>`. A data structure `AttributeCollection` is provided in the `AutoForm.Blazor.Attributes` namespace that makes managing attributes a bit less cumbersome. It is not required in general to use this data structure, as generators should expect any type that is assignable to `IEnumerable<KeyValuePair<String, Object>>` to be returned from an attribute providers methods. We are going to use it here for simplicity.

Create an attributes provider like so:
```cs
public class MyAttributesProvider
{
  public IEnumerable<KeyValuePair<String, Object>> GetNameAttributes()
{
    return new AttributeCollection("placeholder", "Name");
  }

  public IEnumerable<KeyValuePair<String, Object>> GetAgeAttributes()
{
    return new AttributeCollection("min", "0");
  }
}
```
*Found here: [TestApp.AttributesProviders.MyAttributesProvider](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/AttributesProviders/MyAttributesProvider.cs)*

Create a property to provider the attributes provider:

```cs
[Model]
public class Model
{
  public Byte Age { get; set; }
  
  [AutoForm.Attributes.UseTemplate(typeof(Templates.MyTemplate))]
  public String? Name { get; set; }
  
  [AutoForm.Attributes.AttributesProvider]
  public MyAttributesProvider AttributesProvider { get; } = new MyAttributesProvider();
}
```
*Found here: [TestApp.Models.MyModel](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Models/MyModel.cs)*

The control generated for this model will now look something like this:
```cs
private sealed class __Control_TestApp_Models_MyModel : AutoForm.Blazor.Controls.Abstractions.ControlBase<TestApp.Models.MyModel>
{
  protected override void BuildRenderTree(RenderTreeBuilder __builder)
  {
    if(Value != null)
    {
      //Subcontrol for Age
      __builder.OpenComponent<AutoForm.Blazor.Controls.ByteNumber>(0);
      __builder.AddAttribute(1, "Value", RuntimeHelpers.TypeCheck<System.Byte>(Value.Age));
      __builder.AddAttribute(2, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<System.Byte>>(EventCallback.Factory.Create<System.Byte>(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value.Age = __value; ValueChanged.InvokeAsync(Value);}, Value.Age))));
      __builder.AddAttribute(3, "Attributes", RuntimeHelpers.TypeCheck<IEnumerable<KeyValuePair<String, Object>>>(Value.AttributesProvider.GetAgeAttributes()));
      __builder.CloseComponent();
      
      //Template for Name
      __builder.OpenComponent<TestApp.Templates.MyTemplate>(4);
      __builder.AddAttribute(5, "Value", RuntimeHelpers.TypeCheck<System.String>(Value.Name));
      __builder.AddAttribute(6, "Attributes", RuntimeHelpers.TypeCheck<IEnumerable<KeyValuePair<String, Object>>>(Value.AttributesProvider.GetNameAttributes()));
      __builder.AddAttribute(7, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(buildNameSubControl));
      __builder.CloseComponent();

      void buildNameSubControl(RenderTreeBuilder __builder)
      {
        //Subcontrol for Name
        __builder.OpenComponent<AutoForm.Blazor.Controls.Text>(8);
        __builder.AddAttribute(9, "Value", RuntimeHelpers.TypeCheck<System.String>(Value.Name));
        __builder.AddAttribute(10, "ValueChanged", RuntimeHelpers.TypeCheck<EventCallback<System.String>>(EventCallback.Factory.Create<System.String>(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value.Name = __value; ValueChanged.InvokeAsync(Value);}, Value.Name))));
        __builder.AddAttribute(11, "Attributes", RuntimeHelpers.TypeCheck<IEnumerable<KeyValuePair<String, Object>>>(Value.AttributesProvider.GetNameAttributes()));
        __builder.CloseComponent();
      }
    }
  }
}
```

The html rendered will now look something like this (notice the placeholder):

![Image Missing](https://static.rhomicro.com/files/images/github/autoform/3.png)

- - - -
That's it for the basics, look around the `AutoForm.Attributes` namespace to find out more about how to use controls, fallback templates, ordering controls etc. This readme will likely be updated in the future to include more advanced instructions.
