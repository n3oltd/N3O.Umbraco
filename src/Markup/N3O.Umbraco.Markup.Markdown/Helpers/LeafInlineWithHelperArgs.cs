using Markdig.Syntax.Inlines;

namespace N3O.Umbraco.Markup.Markdown.Helpers;

public class LeafInlineWithHelperArgs<T, TArgs> : LeafInline where TArgs : HelperArgs, new() {
    public TArgs HelperArgs { get; set; }
}