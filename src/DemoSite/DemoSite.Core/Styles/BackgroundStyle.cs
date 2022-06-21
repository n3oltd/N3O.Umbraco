using N3O.Umbraco.Templates;

namespace DemoSite.Colours;

public class BackgroundColour : TemplateStyle {
    public BackgroundColour(string id, string name) : base(id, name, null, null) { }

    public override string Icon => "icon-colorpicker";
    
    protected override string NamePrefix => "Background Colour";
}

public class BackgroundColours : ITemplateStylesCollection {
    public static readonly BackgroundColour Green = new("green", "Green");
    public static readonly BackgroundColour Red = new("red", "Red");
}
