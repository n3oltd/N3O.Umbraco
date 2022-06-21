using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Templates;

public class TemplateStyleDataSource : LookupsDataSource<TemplateStyle> {
    public TemplateStyleDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Template Styles";
    public override string Description => "Data source for template styles";
    public override string Icon => "icon-brush";

    protected override string GetDescription(TemplateStyle templateStyle) => templateStyle.Description;
    protected override string GetIcon(TemplateStyle templateStyle) => templateStyle.Icon;
}
