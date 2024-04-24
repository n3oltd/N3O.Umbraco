using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Templates;

public abstract class TemplateStyle : NamedLookup, ITemplateStyle {
    private readonly string _name;
    private readonly IReadOnlyDictionary<string, object> _properties;

    protected TemplateStyle(string id,
                            string name,
                            string description,
                            params KeyValuePair<string, object>[] properties)
        : base(id, name) {
        _name = name;
        Description = description;
        _properties = new Dictionary<string, object>(properties, StringComparer.InvariantCultureIgnoreCase);
    }

    public string Description { get; }
    public override string Name => $"{Category}: {_name}";

    public abstract string Category { get; }
    public abstract string Icon { get; }
    
    public virtual object GetProperty(string propertyAlias) => _properties.GetValueOrDefault(propertyAlias);
}

public interface ITemplateStylesCollection { }

public class TemplateStyles : DistributedLookupsCollection<TemplateStyle, ITemplateStylesCollection> { }
