using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Zakat.Settings.Calculator.Field.Alias)]
public class ZakatCalculatorFieldSettingsContent : UmbracoContent<ZakatCalculatorFieldSettingsContent> {
    public ZakatCalculatorFieldClassification Classification => GetValue(x => x.Classification);
    public ZakatCalculatorFieldType Type => GetValue(x => x.Type);
    public string Alias => GetAs(x => x.Alias);
    public string Name => Content().Name;
    public new IHtmlEncodedString Content => GetValue(x => x.Content);
    public string Tooltip => GetAs(x => x.Tooltip);
    public Metal Metal => GetValue(x => x.Metal);
}