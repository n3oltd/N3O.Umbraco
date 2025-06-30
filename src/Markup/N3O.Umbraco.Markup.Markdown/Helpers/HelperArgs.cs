namespace N3O.Umbraco.Markup.Markdown.Helpers;

public abstract class HelperArgs {
    public string Keyword { get; set; }
}

public class EmptyHelperArgs : HelperArgs { }
