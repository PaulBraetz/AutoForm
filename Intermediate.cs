using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MyDocument
{
	public sealed class Document
	{
		public sealed class Context
		{
			public Context()
			{

			}

			public String TargetFilePath { get; set; }

			public String IntermediateResultFilePath { get; set; }
		}

		public Document(Action<String> print)
		{
			if(print == null)
			{
				throw new ArgumentNullException("print");
			}

			Print = o => print.Invoke(o?.ToString() ?? String.Empty); 
		}

		private readonly Action<Object> Print;
		public Context DocumentContext { get; } = new Context();

		public void Execute()
		{
/*0->6860*/

	//Data
	DocumentContext.IntermediateResultFilePath = @"Intermediate.cs";
    DocumentContext.TargetFilePath = "README.md";

	var template = new
	{
		Source = new FileInfo("README_template.ls"),
		License = (name: "MIT", url: "LICENSE"),
		Name = "AutoForm",
		PrintGenerateDateInfo = true,
		Description = ((Action)description),
		Contributors = new[]
			{
				(name: "Paul Brätz", url: "https://github.com/PaulBraetz/")
			},
		Features = new (string name, Action content)[]
			{
				(name: "Attribute-based model discovery", content:feature1),
				(name: "Debug json generator", content:feature2),
				(name: "Blazor component generator", content:feature3)
			}
	};
	var featureSections = template.Features.Select(f=>(f.name, depth: 2, content:f.content));
	IDictionary<string, (string name, int depth, Action content, Guid id)> sections = null;
	sections = new (string name, int depth, Action content)[]
	{
		(name: "Description", depth: 1, content: description),
		(name: "Table of Contents", depth: 1, content: tableOfContents),
        (name: "Versioning", depth: 1, content:versioning),
		(name: "Features", depth: 1, content: features)
	}.Concat(featureSections)
	.Concat(new (string name, int depth, Action content)[]
	{
		(name: "Installation", depth: 1, content:installation),
        (name: "How To Use", depth: 1, content:howToUse),
        (name: "A Note on Attributes", depth: 2, content:aNoteOnAttributes),
        (name: "Creating Models", depth: 2, content:creatingModels),
        (name: "Creating Templates", depth: 2, content:creatingTemplates),
        (name: "Creating Controls", depth: 2, content:creatingControls),
        (name: "First Results", depth: 2, content:result1),
        (name: "Complex Models", depth: 2, content:complexModels),
        (name: "Complex Models Result", depth: 2, content:result2),
        (name: "Submodels", depth: 2, content:subModels),
        (name: "Submodels Result", depth: 2, content:result3),
        (name: "Planned Features", depth: 1, content:plannedFeatures),
		(name: "License", depth: 1, content:license),
		(name: "Contributors", depth: 1, content:contributors)
	})
	.ToDictionary(t=>t.name, t=>(t.name, t.depth, t.content, id: Guid.NewGuid()));
	//End Data

	//Functions
    void sectionHeader(String name){
        var section = sections[name];
        var tokens = String.Concat(Enumerable.Repeat('#', section.depth+1));
        var line = $"\n\n---\n{tokens} {section.name} <a name=\"{section.id}\"></a>\n\n";
        Print(line);
    }    
    void tableOfContents(){
        var sectionStack = new Stack<Int32>();

        foreach(var section in sections.Values)
        {
            if(sectionStack.Count < section.depth)
            {
                sectionStack.Push(1);
            }else if(sectionStack.Count == section.depth)
            {
                var siblingIndex = sectionStack.Pop();
                sectionStack.Push(siblingIndex + 1);
            }else
            {
                while(sectionStack.Count > section.depth)
                {
                    sectionStack.Pop();
                    var lastOnThisDepth = sectionStack.Pop();
                    sectionStack.Push(lastOnThisDepth + 1);
                }
            }

            var indentation = section.depth > 1 ? "\t" : String.Empty;
            var index = sectionStack.Select(s=>$"{s}.").Aggregate((s1, s2) =>s2+s1);
            var line = $"{indentation}{index} [{section.name}](#{section.id})\n\n";
            Print(line);
        }
    }
	void license(){
		Print($"This software is licensed to you under the [{template.License.name}]({template.License.url}) license.\n");
	}
	void contributors(){
		foreach(var contributor in template.Contributors)
		{
			Print($"* [{contributor.name}]({contributor.url} \"Go to Profile\")\n");
		}
	}
	void features(){
		foreach(var feature in template.Features)
		{
			Print($"* {feature.name}\n");
		}
	}
	//End Functions

	//Generation
	Print($"# {template.Name}\n");
	
	if(template.PrintGenerateDateInfo){
		var timeStamp = DateTimeOffset.UtcNow.ToString(System.Globalization.CultureInfo.GetCultureInfo("de-De"));
		Print($"*Note: this readme was generated on {timeStamp} using {template.Source.Name}*\n\n");
	}
	
	foreach(var section in sections.Values){
		sectionHeader(section.name);
		section.content?.Invoke();
	}
	//End Generation

	//Custom Content
	void installation(){
        Print(@"Currently, there are multiple packages published for generating code based on models:

* For model discovery, an attribute collection is provided in the AutoForm.Attributes package.
* For debug purposes, a json generator is provided in the AutoForm.Json.Analysis package.
* For use in Blazor, a blazor component generator is provided in the AutoForm.Blazor.Analysis package.
* For accessing controls generated by AutoForm.Blazor.Analysis, an entry point control and other default controls and templates are provided in the AutoForm.Blazor package.

In order to use the generators, models have to be annotated using the attributes found in AutoForm.Attributes. Since the generators rely on code semantics in order to discover models, you may implement your own attributes using the same namespace and attribute names as in AutoForm.Attributes. This is currently not reccomended, as the underlying processes are not hardened against faulty attribute declarations."
        );

        var projectRegex = new Regex(@"^.*\.csproj$");
        var versionRegex = new Regex(@"(?<=<(V|v)ersion>)[^<>]*(?=</(V|v)ersion>)");
        var required = new HashSet<String>()
        {
            "AutoForm.Attributes",
            "AutoForm.Blazor",
            "AutoForm.Blazor.Analysis",
            "AutoForm.Json.Analysis"
        };
        var projects = Directory.EnumerateFiles(template.Source.Directory.FullName, "*", SearchOption.AllDirectories)
            .Where(s=>projectRegex.IsMatch(s))
            .Select(s=>new FileInfo(s))
            .Where(i=>required.Contains(Path.GetFileNameWithoutExtension(i.FullName)))
            .Select(i=>
            {
                using var reader = new StreamReader(File.OpenRead(i.FullName));
                var match = versionRegex.Match(reader.ReadToEnd());
                return (file: i, result: match);
            })
            .Where(t=>t.result.Success)
            .Select(t=>(name: Path.GetFileNameWithoutExtension(t.file.FullName), version: t.result.Value));

        foreach(var project in projects)
        {
            Print(
@$"

<details>
<summary>Installing {project.name}</summary>

Nuget Gallery: https://www.nuget.org/packages/RhoMicro.{project.name}

Package Manager: `Install-Package RhoMicro.{project.name} -Version {project.version}`

.Net CLI: `dotnet add package RhoMicro.{project.name} --version {project.version}`
</details>

");
        }
    }
    
	void description() =>

/*6860->7176*/
Print.Invoke(@"
AutoForm is a model-driven UI control (input component) generation tool for .Net. 
Its aim is to decouple UI controls from each other.

Similar to dependency injection, using controls managed by AutoForm forces decoupling from implementations, 
so callers need only know the model for which they require a control.
");
/*7176->7197*/

	void feature1() =>

/*7197->7239*/
Print.Invoke(@"
TODO: describe attribute discovery here.
");
/*7239->7260*/

	void feature2() =>

/*7260->7296*/
Print.Invoke(@"
TODO: describe json generator here
");
/*7296->7317*/

	void feature3() =>

/*7317->7355*/
Print.Invoke(@"
TODO: describe blazor generator here
");
/*7355->7381*/

    void versioning() =>

/*7381->7446*/
Print.Invoke(@"
AutoForm uses [Semantic Versioning 2.0.0](https://semver.org/).
");
/*7446->7481*/
    
    void plannedFeatures() =>

/*7481->7539*/
Print.Invoke(@"
* Importing/Exporting types from and to other assemblies
");
/*7539->8022*/

    void printSample(String name, String caption, String path)
    {
        var codeType = Path.GetExtension(path).Remove(0, 1);
        var url =  @$"https://github.com/PaulBraetz/AutoForm/blob/master/{path.Replace('\\', '/')}";
            
        using var reader = new StreamReader(File.OpenRead(path));
        var content = reader.ReadToEnd();
        Print(
@$"

{caption}

```{codeType}
{content}
```

*Source: [{name}]({url})*

");
    }

    void aNoteOnAttributes() =>

/*8022->8307*/
Print.Invoke(@"
While templates and controls used for the blazor generator are required to provide an `Attributes` property, generated controls will ignore any attributes passed to them.
Default controls found in `AutoForm.Blazor.Controls` will honor attributes passed via the `Attributes` property.
");
/*8307->8332*/


    void howToUse() =>

/*8332->8392*/
Print.Invoke(@"
*The following samples use the provided blazor generator.*
");
/*8392->11813*/


    void creatingModels(){
        foreach(var t in new[]
        {
            (name: "TestApp.Models.MyModel", caption: "Register a model by annotating at least one of its properties with the `ModelPropertyAttribute`:", path:@"TestApp\Models\MyModel.cs"),
            (name: "TestApp.Pages.Index.razor", caption: "Access the generated control using the `AutoControl`:", path: @"TestApp\Pages\Index.razor")
        })
        {
            printSample(t.name, t.caption, t.path);
        }
        
        Print(
@"
Note that default controls provided by `AutoForm.Blazor`, found in `AutoForm.Blazor.Controls`, will bind to the `oninput` event. 
This means that when using these controls, models will be updated with every keystroke instead of when a control loses focus, as is default.
"
        );
    }

    void creatingTemplates(){
        var name = "AutoForm.Blazor.Templates.Abstractions.TemplateBase";
        var caption = "Templates may be created in order surround a control with another component. They must semantically implement the following interface:";
        var path = @"AutoForm.Blazor\Templates\Abstractions\TemplateBase.cs";
        printSample(name, caption, path);

        Print(
@"Here, TModel is the model type whose control this template should be applied to. These properties enable the template to access the controls attributes as well as the models current value. The control will be passed to `ChildContent`.

*Note that templates must only provide properties that are semantically identical to those found in `TemplateBase`, as well as inherit from `ComponentBase`.*"
        );

        foreach(var t in new []
        {
            (name: "TestApp.Templates.MyTemplate", caption: "For uniform bootstrap template styling, a primitive base template may be created like so:", path:@"TestApp\Templates\MyTemplate.razor"),
            (name: "TestApp.Templates.MyTemplate", caption: String.Empty, path:@"TestApp\Templates\MyTemplate.razor.cs"),
            (name: "TestApp.Templates.MyModelNameTemplate", caption: "Implement a template for controls whose model is of type `System.String` or that control the `MyModel.Name` property:", path:@"TestApp\Templates\StringTemplate.cs"),
        })
        {
            printSample(t.name, t.caption, t.path);
        }
    }

    void creatingControls(){
        var name = "AutoForm.Blazor.Controls.Abstractions.ControlBase";
        var caption = "Controls may be declared in order to override default or generated controls. They must semantically implement the following interface:";
        var path = @"AutoForm.Blazor\Controls\Abstractions\ControlBase.cs";
        printSample(name, caption, path);

        Print(
@"Here, TModel is the model type this control should be applied to.

*Note that controls must only provide properties that are semantically identical to those found in `ControlBase`, as well as inherit from `ComponentBase`.*"
        );

        foreach(var t in new []
        {
            (name: "TestApp.Controls.MyModelNameControl", caption: "Implement a control for models of type `System.String` or subcontrol for `MyModel.Name`:", path:@"TestApp\Controls\StringControl.razor"),
            (name: "TestApp.Controls.MyModelNameControl", caption: "", path:@"TestApp\Controls\StringControl.razor.cs")
        })
        {
            printSample(t.name, t.caption, t.path);
        }
    }

    void result1() =>

/*11813->12012*/
Print.Invoke(@"
The html rendered by `AutoForm.Blazor.AutoControl` for models of type `MyModel` will now look something like this:

![Missing Image](https://static.rhomicro.com/files/images/github/autoform/10.png)
");
/*12012->12715*/

    void complexModels(){
        foreach(var t in new[]
        {
            (name: "TestApp.Models.ComplexModel.cs", 
            caption: 
    @"Models require all their marked properties to be assignable to a control. 
    This means thet composite models will require all their submodels to also be marked for control generation. 
    Note that this model requires a subcontrol for its `NestedModel` property. 
    Luckily, its type has already been marked as a model. Had it not been, the generator would have issued an error.", 
            path: @"TestApp\Models\ComplexModel.cs")
        })
        {
            printSample(t.name, t.caption, t.path);
        }
    }

    void result2() =>

/*12715->12919*/
Print.Invoke(@"
The html rendered by `AutoForm.Blazor.AutoControl` for models of type `ComplexModel` will now look something like this:

![Missing Image](https://static.rhomicro.com/files/images/github/autoform/20.png)
");
/*12919->13812*/

    void subModels(){
        foreach(var t in new[]
        {
            (name: "TestApp.Models.SubModel.cs", 
            caption: @"Models may inherit their base types model properties. Annotating a model with the `SubModelAttribute` will instruct the generator to enhance the generated control with subcontrols for properties found in the basetype.", 
            path: @"TestApp\Models\SubModel.cs"),
            (name: "TestApp.Templates.SubModel.NameTemplate.cs",
            caption: "For this model, labeled subcontrols are created using templates:",
            path: @"TestApp\Templates\SubModel\NameTemplate.cs"),
            (name: "TestApp.Templates.SubModel.AgeTemplate.cs",
            caption: "",
            path: @"TestApp\Templates\SubModel\AgeTemplate.cs")
        })
        {
            printSample(t.name, t.caption, t.path);
        }
    }

    void result3() =>

/*13812->14011*/
Print.Invoke(@"
The html rendered by `AutoForm.Blazor.AutoControl` for models of type `SubModel` will now look something like this:

![Missing Image](https://static.rhomicro.com/files/images/github/autoform/40.png)");

		}
	}
}