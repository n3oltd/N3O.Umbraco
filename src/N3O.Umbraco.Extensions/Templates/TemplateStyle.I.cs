namespace N3O.Umbraco.Templates;

public interface ITemplateStyle {
    string Category { get; }
    string CssClass { get; }
    string Description { get; }
    string Icon { get; }
    string Name { get; }
}