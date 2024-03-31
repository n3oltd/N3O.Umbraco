using Humanizer;

namespace N3O.Umbraco.Templates;

public static class TemplateStyleId {
    public static string Generate(string category, string name) {
        return $"{category.Camelize()}_{name.Camelize()}";
    }
}