namespace N3O.Umbraco.Templates;

public interface ITemplateEngine {
    bool IsSyntaxValid(string content);
    string Render(string markup, object model);
}
