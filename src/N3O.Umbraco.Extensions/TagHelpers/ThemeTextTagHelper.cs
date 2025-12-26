using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}theme-text")]
public class ThemeTextTagHelper : TagHelper {
    private readonly IJsonProvider _jsonProvider;
    private readonly IStringLocalizer _stringLocalizer;

    [HtmlAttributeName("path")]
    public string Path { get; set; }
    
    [HtmlAttributeName("strings")]
    public Type StringsType { get; set; }

    public ThemeTextTagHelper(IJsonProvider jsonProvider, IStringLocalizer stringLocalizer) {
        _jsonProvider = jsonProvider;
        _stringLocalizer = stringLocalizer;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (!StringsType.ImplementsInterface<IStrings>() || !StringsType.HasParameterlessConstructor()) {
            throw new ArgumentException(nameof(StringsType));
        }
        
        var strings = (IStrings) Activator.CreateInstance(StringsType);
        var serializerSettings = _jsonProvider.GetSettings();
        var serializer = JsonSerializer.Create(serializerSettings);
        serializer.ContractResolver = new TypeOnlyContractResolver(StringsType);
        
        var jObject = JObject.FromObject(strings, serializer);

        foreach (var (key, value) in jObject) {
            jObject[key] = _stringLocalizer.Get(strings.Folder, strings.Name, (string) value);
        }
        
        var javascript = $"window.themeConfig.text.{Path.Camelize()} = {jObject}";
        
        output.TagName = null;

        var scriptTag = new TagBuilder("script");

        scriptTag.InnerHtml.AppendHtmlLine(javascript);

        output.Content.SetHtmlContent(scriptTag.ToHtmlString());
    }
}
