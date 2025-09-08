using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

namespace N3O.Umbraco.Templates;

public class MergerHtmlContent : IHtmlContent {
    private readonly IHtmlContent _htmlContent;
    private readonly ITemplateEngine _templateEngine;
    private readonly IReadOnlyDictionary<string, object> _mergeModels;

    public MergerHtmlContent(ITemplateEngine templateEngine,
                             IHtmlContent htmlContent,
                             IReadOnlyDictionary<string, object> mergeModels) {
        _htmlContent = htmlContent;
        _templateEngine = templateEngine;
        _mergeModels = mergeModels;
    }

    public void WriteTo(TextWriter writer, HtmlEncoder encoder) {
        using (var stringWriter = new StringWriter()) {
            _htmlContent.WriteTo(stringWriter, encoder);
            
            var markup = stringWriter.ToString();
            var html = _templateEngine.Render(markup, _mergeModels);
            
            writer.WriteAsync(html);
        }
    }
}