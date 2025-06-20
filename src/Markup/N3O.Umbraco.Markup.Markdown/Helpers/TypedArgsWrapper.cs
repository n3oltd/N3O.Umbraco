using Markdig.Syntax.Inlines;

namespace N3O.Umbraco.Markup.Markdown.Helpers;

public class TypedArgsWrapper<T, TArgs> : LeafInline where TArgs : HelperArgs, new() {
    public TArgs HelperArgs { get; set; }
}