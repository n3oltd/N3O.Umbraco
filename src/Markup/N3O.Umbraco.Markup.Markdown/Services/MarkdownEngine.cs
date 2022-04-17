using Markdig;
using Microsoft.AspNetCore.Html;

namespace N3O.Umbraco.Markup.Markdown {
    public class MarkdownEngine : IMarkupEngine {
        private readonly MarkdownPipeline _pipeline;

        public MarkdownEngine(MarkdownPipeline pipeline) {
            _pipeline = pipeline;
        }

        public HtmlString RenderHtml(string markup) {
            var html = Markdig.Markdown.ToHtml(markup, _pipeline);

            return new HtmlString(html);
        }

        public bool Validate(string markup) {
            try {
                RenderHtml(markup);

                return true;
            } catch {
                return false;
            }
        }
    }
}