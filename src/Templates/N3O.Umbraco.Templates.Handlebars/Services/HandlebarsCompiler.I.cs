using HandlebarsDotNet;

namespace N3O.Umbraco.Templates.Handlebars;

public interface IHandlebarsCompiler {
    bool IsSyntaxValid(string markup);
    HandlebarsTemplate<object, object> Compile(string markup, string cacheKey = null);
}
