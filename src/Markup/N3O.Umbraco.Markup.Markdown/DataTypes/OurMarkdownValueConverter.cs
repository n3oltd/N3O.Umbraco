using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Templates;
using Umbraco.Extensions;

namespace N3O.Umbraco.Markup.Markdown.DataTypes;

public class OurMarkdownValueConverter : MarkdownEditorValueConverter {
    private readonly IMarkupEngine _markupEngine;

    public OurMarkdownValueConverter(HtmlLocalLinkParser localLinkParser,
                                     HtmlUrlParser urlParser,
                                     IMarkupEngine markupEngine)
        : base(localLinkParser, urlParser) {
        _markupEngine = markupEngine;
    }

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) => typeof(IHtmlEncodedString);

    public override object ConvertIntermediateToObject(IPublishedElement owner,
                                                       IPublishedPropertyType propertyType,
                                                       PropertyCacheLevel referenceCacheLevel,
                                                       object inter,
                                                       bool preview) {
        var html = default(IHtmlEncodedString);
        
        if (inter is string markdown) {
            html = _markupEngine.RenderHtml(markdown).IfNotNull(x => new HtmlEncodedString(x.ToString()));
        }
        
        return html;
    }
}