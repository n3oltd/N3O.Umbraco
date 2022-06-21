using Markdig;
using Microsoft.AspNetCore.Html;

namespace N3O.Umbraco.Markup.Markdown;

public class MarkdownEngine : IMarkupEngine {
    private readonly MarkdownPipeline _pipeline;

    public MarkdownEngine(MarkdownPipeline pipeline) {
        _pipeline = pipeline;
    }

    public HtmlString RenderHtml(string content) {
        var html = Markdig.Markdown.ToHtml(content, _pipeline);

        return new HtmlString(html);
    }

    public bool Validate(string content) {
        try {
            RenderHtml(content);

            return true;
        } catch {
            return false;
        }
    }
}
