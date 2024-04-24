namespace N3O.Umbraco.Templates;

public interface ITemplateStyle {
    string Id { get; }
    string Category { get; }
    string Description { get; }
    string Icon { get; }
    string Name { get; }
    
    object GetProperty(string propertyAlias);
}