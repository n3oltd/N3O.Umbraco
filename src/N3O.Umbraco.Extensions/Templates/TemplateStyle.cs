using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Templates;

public abstract class TemplateStyle : NamedLookup {
    private readonly string _name;

    protected TemplateStyle(string id, string name, string description, string cssClass) : base(id, name) {
        _name = name;
        Description = description;
        CssClass = cssClass;
    }

    public string Description { get; }
    public string CssClass { get; }
    public override string Name => $"{NamePrefix}: {_name}";

    public abstract string Icon { get; }
    
    protected abstract string NamePrefix { get; }
}

public interface ITemplateStylesCollection { }

public class TemplateStyles : DistributedLookupsCollection<TemplateStyle, ITemplateStylesCollection> { }
