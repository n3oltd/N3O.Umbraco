using Markdig.Syntax.Inlines;

namespace N3O.Umbraco.Markup.Markdown.Helpers;

public abstract class HelperArgs : LeafInline {
    public string Keyword { get; set; }
}

public class EmptyHelperArgs : HelperArgs { }
