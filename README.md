
# AutoForm #

AutoForm is a model-driven UI control (input component) generation tool for .Net. 
Its aim is to decouple UI controls from each other.

Similar to dependency injection, using controls managed by AutoForm forces decoupling from implementations, 
so callers need only know the model for which they require a control.

*Note: this readme has been generated on 13.11.2022 23:49:09 +01:00*

---
## **Features** ##

* Attribute-based model discovery
* Debug json generator
* Blazor component generator

---
## **Versioning** ##

AutoForm uses [Semantic Versioning 2.0.0](https://semver.org/).

---
## **Installation** ##

Currently, there are multiple packages published for generating code based on models:

* For model discovery, an attribute collection is provided in the AutoForm.Attributes package.
* For debug purposes, a json generator is provided in the AutoForm.Json.Analysis package.
* For use in Blazor, a blazor component generator is provided in the AutoForm.Blazor.Analysis package.
* For accessing controls generated by AutoForm.Blazor.Analysis, an entry point control and other default controls and templates are provided in the AutoForm.Blazor package.

In order to use the generators, models have to be annotated using the attributes found in AutoForm.Attributes. Since the generators rely on code semantics in order to discover models, you may implement your own attributes using the same namespace and attribute names as in AutoForm.Attributes. This is currently not reccomended, as the underlying processes are not hardened against faulty attribute declarations.


<details>
<summary>Installing AutoForm.Attributes</summary>

Nuget Gallery: https://www.nuget.org/packages/RhoMicro.AutoForm.Attributes

Package Manager: `Install-Package RhoMicro.AutoForm.Attributes -Version 3.0.0`

.Net CLI: `dotnet add package RhoMicro.AutoForm.Attributes --version 3.0.0`
</details>

<details>
<summary>Installing AutoForm.Blazor</summary>

Nuget Gallery: https://www.nuget.org/packages/RhoMicro.AutoForm.Blazor

Package Manager: `Install-Package RhoMicro.AutoForm.Blazor -Version 2.1.0`

.Net CLI: `dotnet add package RhoMicro.AutoForm.Blazor --version 2.1.0`
</details>

<details>
<summary>Installing AutoForm.Blazor.Analysis</summary>

Nuget Gallery: https://www.nuget.org/packages/RhoMicro.AutoForm.Blazor.Analysis

Package Manager: `Install-Package RhoMicro.AutoForm.Blazor.Analysis -Version 3.0.1`

.Net CLI: `dotnet add package RhoMicro.AutoForm.Blazor.Analysis --version 3.0.1`
</details>

<details>
<summary>Installing AutoForm.Json.Analysis</summary>

Nuget Gallery: https://www.nuget.org/packages/RhoMicro.AutoForm.Json.Analysis

Package Manager: `Install-Package RhoMicro.AutoForm.Json.Analysis -Version 3.0.1`

.Net CLI: `dotnet add package RhoMicro.AutoForm.Json.Analysis --version 3.0.1`
</details>

---
## **How To Use** ##

*The following samples use the provided blazor generator.*

### **A Note on Attributes** ###

While templates and controls used for the blazor generator are required to provide an `Attributes` property, generated controls will ignore any attributes passed to them.
Default controls found in `AutoForm.Blazor.Controls` will honor attributes passed via the `Attributes` property.

### **Creating Models** ###

Register a model by annotating at least one of its properties with the `ModelPropertyAttribute`:

```cs
using AutoForm.Attributes;

namespace TestApp.Models
{
	public class MyModel
	{
		[ModelProperty]
		public String? Name { get; set; }
	}
}

```

*Source: [TestApp.Models.MyModel](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Models/MyModel.cs)*

Access the generated control using the `AutoControl`:

```razor
@page "/"
@using TestApp.Models

<AutoForm.Blazor.AutoControl @bind-Value="Model" />

<span>
	@(nameof(MyModel.Name)) : @(Model.Name)
</span>

@code {
	private MyModel Model { get; set; } = new MyModel();
}
```

*Source: [TestApp.Pages.Index.razor](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Pages/Index.razor)*

Note that default controls provided by `AutoForm.Blazor`, found in `AutoForm.Blazor.Controls`, will bind to the `oninput` event. 
This means that when using these controls, models will be updated with every keystroke instead of when a control loses focus, as is default.

### **Creating Templates** ###

Templates may be created in order surround a control with another component. They must semantically implement the following interface:

```cs
using Microsoft.AspNetCore.Components;

namespace AutoForm.Blazor.Templates.Abstractions
{
	public abstract class TemplateBase<TModel> : ComponentBase
	{
		[Parameter]
		public virtual TModel? Value { get; set; }
		[Parameter]
		public virtual IEnumerable<KeyValuePair<String, Object>>? Attributes { get; set; }
		[Parameter]
		public virtual RenderFragment? ChildContent { get; set; }
	}
}

```

*Source: [AutoForm.Blazor.Templates.Abstractions.TemplateBase](https://github.com/PaulBraetz/AutoForm/blob/master/AutoForm.Blazor/Templates/Abstractions/TemplateBase.cs)*

Here, TModel is the model type whose control this template should be applied to. These properties enable the template to access the controls attributes as well as the models current value. The control will be passed to `ChildContent`.

*Note that templates must only provide properties that are semantically identical to those found in `TemplateBase`, as well as inherit from `ComponentBase`.*



For uniform bootstrap template styling, a primitive base template may be created like so:

```razor
@typeparam TModel
@inherits AutoForm.Blazor.Templates.Abstractions.TemplateBase<TModel>

<div class="input-group mb-3">
	<span class="input-group-text" >@Text</span>
	@ChildContent
</div>

@code{
	private Object Text => Attributes?.SingleOrDefault(kvp => kvp.Key == "label").Value ??
								"Enter Value:";
}
```

*Source: [TestApp.Templates.MyTemplate](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Templates/MyTemplate.razor)*



```cs
using AutoForm.Attributes;
using AutoForm.Blazor.Templates.Abstractions;
using TestApp.Models;

namespace TestApp.Templates
{
	public abstract partial class MyTemplate<TModel> : TemplateBase<TModel>
	{
	}
}

```

*Source: [TestApp.Templates.MyTemplate](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Templates/MyTemplate.razor.cs)*

Implement a template for controls whose model is of type `System.String` or that control the `MyModel.Name` property:

```cs
using AutoForm.Attributes;
using TestApp.Models;

namespace TestApp.Templates
{
	[DefaultTemplate(typeof(String))]
	[DefaultTemplate(typeof(MyModel), nameof(MyModel.Name))]
	public sealed class StringTemplate : MyTemplate<String> { }
}

```

*Source: [TestApp.Templates.MyModelNameTemplate](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Templates/StringTemplate.cs)*

### **Creating Controls** ###

Controls may be declared in order to override default or generated controls. They must semantically implement the following interface:

```cs
using Microsoft.AspNetCore.Components;

namespace AutoForm.Blazor.Controls.Abstractions
{
	public abstract class ControlBase<TModel> : ComponentBase
	{
		[Parameter]
		public virtual TModel? Value { get; set; }

		[Parameter]
		public virtual EventCallback<TModel> ValueChanged { get; set; }

		[Parameter]
		public virtual IEnumerable<KeyValuePair<String, Object>>? Attributes { get; set; }
	}
}

```

*Source: [AutoForm.Blazor.Controls.Abstractions.ControlBase](https://github.com/PaulBraetz/AutoForm/blob/master/AutoForm.Blazor/Controls/Abstractions/ControlBase.cs)*

Here, TModel is the model type this control should be applied to.

*Note that controls must only provide properties that are semantically identical to those found in `ControlBase`, as well as inherit from `ComponentBase`.*



Implement a control for models of type `System.String` or subcontrol for `MyModel.Name`:

```razor
@inherits AutoForm.Blazor.Controls.Abstractions.ControlBase<String>

<input class="form-control" type="text" @attributes="Attributes" value="@Value" @onchange="v=>ValueChanged.InvokeAsync(Value = v.Value?.ToString())" />
```

*Source: [TestApp.Controls.MyModelNameControl](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Controls/StringControl.razor)*



```cs
using AutoForm.Attributes;
using TestApp.Models;

namespace TestApp.Controls
{
	[DefaultControl(typeof(String))]
	[DefaultControl(typeof(MyModel), nameof(MyModel.Name))]
	public partial class StringControl: AutoForm.Blazor.Controls.Abstractions.ControlBase<String>
	{
	}
}

```

*Source: [TestApp.Controls.MyModelNameControl](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Controls/StringControl.razor.cs)*

### **Result** ###

The html rendered by `AutoForm.Blazor.AutoControl` for modely of type `MyModel` will now look something like this:

![Missing Image](https://static.rhomicro.com/files/images/github/autoform/1.png)

### **Complex Models** ###



Models require all their marked properties to be assignable to a control. 
This means thet composite models will require all their submodels to also be marked for control generation. 
Note that this model requires a subcontrol for its `NestedModel` property. 
Luckily, its type has already been marked as a model. Had it not been, the generator would have issued an error.

```cs
using AutoForm.Attributes;

namespace TestApp.Models
{
	public sealed class ComplexModel
	{
		[ModelProperty]
		public String? Street { get; set; }
		[ModelProperty]
		public String? City { get; set; }
		[ModelProperty]
		public Int32 ZipCode { get; set; }
		[ModelProperty]
		public String? Address { get; set; }
		[ModelProperty]
		public MyModel NestedModel { get; set; } = new MyModel();
	}
}

```

*Source: [TestApp.Models.ComplexModel.cs](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Models/ComplexModel.cs)*

### **Result** ###

The html rendered by `AutoForm.Blazor.AutoControl` for modely of type `ComplexModel` will now look something like this:

![Missing Image](https://static.rhomicro.com/files/images/github/autoform/2.png)

### **Sub Models** ###



Models may inherit their base types model properties. 
Annotating a model with the `SubModelAttribute` will instruct the generator to enhance the generated control with subcontrols for properties found in the basetype.

```cs
using AutoForm.Attributes;

namespace TestApp.Models
{
	//Inherit subcontrols for all properties of MyModel
	[SubModel(typeof(MyModel))]
	//Inherit subcontrols for specific properties in MyModel
	[SubModel(typeof(MyModel), nameof(MyModel.Name))]
	public class SubModel : MyModel
	{
		[ModelProperty]
		public Int32 Age { get; set; }
	}
}

```

*Source: [TestApp.Models.SubModel.cs](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Models/SubModel.cs)*

For this model, labeled subcontrols are created using templates:

```cs
using AutoForm.Attributes;

namespace TestApp.Templates.SubModel
{
	[DefaultTemplate(typeof(Models.SubModel), nameof(Models.SubModel.Name))]
	public sealed class NameTemplate : MyTemplate<string>
    {
        public override IEnumerable<KeyValuePair<string, object>>? Attributes { get; set; }
            = new Dictionary<string, object>()
            {
                {"label", nameof(Models.SubModel.Name) }
            };
    }
}

```

*Source: [TestApp.Templates.SubModel.NameTemplate.cs](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Templates/SubModel/NameTemplate.cs)*



```cs
using AutoForm.Attributes;

namespace TestApp.Templates.SubModel
{
    [DefaultTemplate(typeof(Models.SubModel), nameof(Models.SubModel.Age))]
    public sealed class AgeTemplate : MyTemplate<int>
    {
        public override IEnumerable<KeyValuePair<string, object>>? Attributes { get; set; }
            = new Dictionary<string, object>()
            {
                {"label", nameof(Models.SubModel.Age) }
            };
    }
}

```

*Source: [TestApp.Templates.SubModel.AgeTemplate.cs](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Templates/SubModel/AgeTemplate.cs)*

### **Result** ###

The html rendered by `AutoForm.Blazor.AutoControl` for modely of type `SubModel` will now look something like this:

![Missing Image](https://static.rhomicro.com/files/images/github/autoform/3.png)

---

## **Planned Features** ##

* Importing/Exporting types from and to other assemblies

---

## **License** ##

This project is licensed to you under the [MIT License](https://github.com/PaulBraetz/AutoForm/blob/master/LICENSE)

---

## **Contributors** ##

* [Paul Braetz](https://github.com/PaulBraetz/)

---