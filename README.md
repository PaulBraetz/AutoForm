ParserInfo.PackageName:CSharpParser
ParserInfo.PackageVersion:0.0.1
ParserInfo.Arguments:-s README_source.ls -t README_dom.json
ParserInfo.PackageHash:/vIX6UeP/VK9bkvHl9WnpQ==
ParserInfo.PackageHash.Algorithm.Name:md5

InterpreterInfo.PackageName:CSharpInterpreter
InterpreterInfo.PackageVersion:0.0.1
InterpreterInfo.Arguments:-s README_dom.json
InterpreterInfo.PackageHash:sKNuClGw26LrIFcTW2fs/w==
InterpreterInfo.PackageHash.Algorithm.Name:md5

# AutoForm
*Note: this readme was generated on 30.01.2023 16:49:29 +00:00 using README_template.ls*



---
## Description <a name="8102b669-b85e-4b09-9e2d-599e1ccfe305"></a>


AutoForm is a model-driven UI control (input component) generation tool for .Net. 
Its aim is to decouple UI controls from each other.

Similar to dependency injection, using controls managed by AutoForm forces decoupling from implementations, 
so callers need only know the model for which they require a control.


---
## Table of Contents <a name="0c854e9e-3797-4336-824a-b39c90d32f48"></a>

1. [Description](#8102b669-b85e-4b09-9e2d-599e1ccfe305)

2. [Table of Contents](#0c854e9e-3797-4336-824a-b39c90d32f48)

3. [Versioning](#a39ff034-9fcd-431d-ae09-feb58a35b5d1)

4. [Features](#3cb9bff6-cce2-460f-a178-203f8bfe2745)

	4.1. [Attribute-based model discovery](#f0eb8818-0caa-4e67-bf54-62297d369713)

	4.2. [Debug json generator](#cba7ee9f-e1ac-4cb6-a003-1b00a4267a7a)

	4.3. [Blazor component generator](#6329563d-0773-47bb-8913-2c08cc95bbc0)

5. [Installation](#7276a80a-60fc-4d08-90b6-1727646b454b)

6. [How To Use](#345695c9-983d-43fe-a55b-748cdba5e3ab)

	6.1. [A Note on Attributes](#62a1abc8-03ed-4ae8-ab58-2ee7e8171852)

	6.2. [Creating Models](#8cc2bb48-bbb9-480e-bfb9-8554f674c9ef)

	6.3. [Creating Templates](#c8a37889-e2b6-4e2a-af65-04de04c5690d)

	6.4. [Creating Controls](#e26afc87-81e8-4ff3-af4b-df9e82a8b53a)

	6.5. [First Results](#638449c6-b74f-40e6-9a53-3a17a513be77)

	6.6. [Complex Models](#31703372-727a-4266-a3b8-025a4f1d0bc0)

	6.7. [Complex Models Result](#c9b3adec-990d-4720-8af3-004d46201437)

	6.8. [Submodels](#ea791b1f-0560-44b0-8b29-cf899e4af1dc)

	6.9. [Submodels Result](#f0af0105-49f8-4e4f-ac77-44c645fa3922)

7. [Planned Features](#cf8abe66-e816-4971-9c1f-5524dc62c0d7)

8. [License](#d41b5c27-0446-4d23-ae8b-114b9ae422e3)

9. [Contributors](#16194d99-1fb8-40a1-a692-c12ef5f6d3b1)



---
## Versioning <a name="a39ff034-9fcd-431d-ae09-feb58a35b5d1"></a>


AutoForm uses [Semantic Versioning 2.0.0](https://semver.org/).


---
## Features <a name="3cb9bff6-cce2-460f-a178-203f8bfe2745"></a>

* Attribute-based model discovery
* Debug json generator
* Blazor component generator


---
### Attribute-based model discovery <a name="f0eb8818-0caa-4e67-bf54-62297d369713"></a>


TODO: describe attribute discovery here.


---
### Debug json generator <a name="cba7ee9f-e1ac-4cb6-a003-1b00a4267a7a"></a>


TODO: describe json generator here


---
### Blazor component generator <a name="6329563d-0773-47bb-8913-2c08cc95bbc0"></a>


TODO: describe blazor generator here


---
## Installation <a name="7276a80a-60fc-4d08-90b6-1727646b454b"></a>

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
## How To Use <a name="345695c9-983d-43fe-a55b-748cdba5e3ab"></a>


*The following samples use the provided blazor generator.*


---
### A Note on Attributes <a name="62a1abc8-03ed-4ae8-ab58-2ee7e8171852"></a>


While templates and controls used for the blazor generator are required to provide an `Attributes` property, generated controls will ignore any attributes passed to them.
Default controls found in `AutoForm.Blazor.Controls` will honor attributes passed via the `Attributes` property.


---
### Creating Models <a name="8cc2bb48-bbb9-480e-bfb9-8554f674c9ef"></a>



Register a model by annotating at least one of its properties with the `ModelPropertyAttribute`:

```cs
using AutoForm.Attributes;

namespace TestApp.Models;

public class MyModel
{
	[ModelProperty]
	public String? Name {
		get;
		set;
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


---
### Creating Templates <a name="c8a37889-e2b6-4e2a-af65-04de04c5690d"></a>



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
using AutoForm.Blazor.Templates.Abstractions;

namespace TestApp.Templates;

public abstract partial class MyTemplate<TModel> : TemplateBase<TModel>
{
}

```

*Source: [TestApp.Templates.MyTemplate](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Templates/MyTemplate.razor.cs)*



Implement a template for controls whose model is of type `System.String` or that control the `MyModel.Name` property:

```cs
using AutoForm.Attributes;

using TestApp.Models;

namespace TestApp.Templates;

[DefaultTemplate(typeof(String))]
[DefaultTemplate(typeof(MyModel), nameof(MyModel.Name))]
public sealed class StringTemplate : MyTemplate<String>
{
}

```

*Source: [TestApp.Templates.MyModelNameTemplate](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Templates/StringTemplate.cs)*



---
### Creating Controls <a name="e26afc87-81e8-4ff3-af4b-df9e82a8b53a"></a>



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

namespace TestApp.Controls;

[DefaultControl(typeof(String))]
[DefaultControl(typeof(MyModel), nameof(MyModel.Name))]
public partial class StringControl : AutoForm.Blazor.Controls.Abstractions.ControlBase<String>
{
}

```

*Source: [TestApp.Controls.MyModelNameControl](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Controls/StringControl.razor.cs)*



---
### First Results <a name="638449c6-b74f-40e6-9a53-3a17a513be77"></a>


The html rendered by `AutoForm.Blazor.AutoControl` for models of type `MyModel` will now look something like this:

![Missing Image](https://static.rhomicro.com/files/images/github/autoform/10.png)


---
### Complex Models <a name="31703372-727a-4266-a3b8-025a4f1d0bc0"></a>



Models require all their marked properties to be assignable to a control. 
    This means thet composite models will require all their submodels to also be marked for control generation. 
    Note that this model requires a subcontrol for its `NestedModel` property. 
    Luckily, its type has already been marked as a model. Had it not been, the generator would have issued an error.

```cs
using AutoForm.Attributes;

namespace TestApp.Models;

public sealed class ComplexModel
{
	[ModelProperty]
	public String? Street {
		get; set;
	}
	[ModelProperty]
	public String? City {
		get; set;
	}
	[ModelProperty]
	public Int32 ZipCode {
		get; set;
	}
	[ModelProperty]
	public String? Address {
		get; set;
	}
	[ModelProperty]
	public MyModel NestedModel { get; set; } = new MyModel();
}

```

*Source: [TestApp.Models.ComplexModel.cs](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Models/ComplexModel.cs)*



---
### Complex Models Result <a name="c9b3adec-990d-4720-8af3-004d46201437"></a>


The html rendered by `AutoForm.Blazor.AutoControl` for models of type `ComplexModel` will now look something like this:

![Missing Image](https://static.rhomicro.com/files/images/github/autoform/20.png)


---
### Submodels <a name="ea791b1f-0560-44b0-8b29-cf899e4af1dc"></a>



Models may inherit their base types model properties. Annotating a model with the `SubModelAttribute` will instruct the generator to enhance the generated control with subcontrols for properties found in the basetype.

```cs
using AutoForm.Attributes;

namespace TestApp.Models;

//Inherit subcontrols for all properties of MyModel
[SubModel(typeof(MyModel))]
//Inherit subcontrols for specific properties in MyModel
[SubModel(typeof(MyModel), nameof(MyModel.Name))]
public class SubModel : MyModel
{
	[ModelProperty]
	public Int32 Age {
		get; set;
	}
}

```

*Source: [TestApp.Models.SubModel.cs](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Models/SubModel.cs)*



For this model, labeled subcontrols are created using templates:

```cs
using AutoForm.Attributes;

namespace TestApp.Templates.SubModel;

[DefaultTemplate(typeof(Models.SubModel), nameof(Models.SubModel.Name))]
public sealed class NameTemplate : MyTemplate<String>
{
	public override IEnumerable<KeyValuePair<String, Object>>? Attributes {
		get; set;
	}
		= new Dictionary<String, Object>()
		{
				{"label", nameof(Models.SubModel.Name) }
		};
}

```

*Source: [TestApp.Templates.SubModel.NameTemplate.cs](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Templates/SubModel/NameTemplate.cs)*





```cs
using AutoForm.Attributes;

namespace TestApp.Templates.SubModel;

[DefaultTemplate(typeof(Models.SubModel), nameof(Models.SubModel.Age))]
public sealed class AgeTemplate : MyTemplate<Int32>
{
	public override IEnumerable<KeyValuePair<String, Object>>? Attributes {
		get; set;
	}
		= new Dictionary<String, Object>()
		{
				{"label", nameof(Models.SubModel.Age) }
		};
}

```

*Source: [TestApp.Templates.SubModel.AgeTemplate.cs](https://github.com/PaulBraetz/AutoForm/blob/master/TestApp/Templates/SubModel/AgeTemplate.cs)*



---
### Submodels Result <a name="f0af0105-49f8-4e4f-ac77-44c645fa3922"></a>


The html rendered by `AutoForm.Blazor.AutoControl` for models of type `SubModel` will now look something like this:

![Missing Image](https://static.rhomicro.com/files/images/github/autoform/40.png)

---
## Planned Features <a name="cf8abe66-e816-4971-9c1f-5524dc62c0d7"></a>


* Importing/Exporting types from and to other assemblies


---
## License <a name="d41b5c27-0446-4d23-ae8b-114b9ae422e3"></a>

This software is licensed to you under the [MIT](LICENSE) license.


---
## Contributors <a name="16194d99-1fb8-40a1-a692-c12ef5f6d3b1"></a>

* [Paul Brätz](https://github.com/PaulBraetz/ "Go to Profile")
