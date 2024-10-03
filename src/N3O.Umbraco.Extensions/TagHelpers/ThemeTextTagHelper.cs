using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement("n3o-theme-text")]
public class ThemeTextTagHelper : TagHelper {
    private readonly IJsonProvider _jsonProvider;

    [HtmlAttributeName("path")]
    public string Path { get; set; }
    
    [HtmlAttributeName("strings")]
    public Type Strings { get; set; }

    public ThemeTextTagHelper(IJsonProvider jsonProvider) {
        _jsonProvider = jsonProvider;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (!Strings.ImplementsInterface<IStrings>() || !Strings.HasParameterlessConstructor()) {
            throw new ArgumentException(nameof(Strings));
        }

        var strings = Activator.CreateInstance(Strings);
        var serializerSettings = _jsonProvider.GetSettings();
        var serializer = JsonSerializer.Create(serializerSettings);
        
        var jObject = JObject.FromObject(strings, serializer);
        jObject.Remove(nameof(IStrings.Folder).Camelize());
        jObject.Remove(nameof(IStrings.Name).Camelize());
        
        var javascript = $"window.themeConfig.text.{Path.Camelize()} = {jObject}";
        
        output.TagName = null;

        var scriptTag = new TagBuilder("script");

        scriptTag.InnerHtml.AppendHtmlLine(javascript);

        output.Content.SetHtmlContent(scriptTag.ToHtmlString());
    }
}
