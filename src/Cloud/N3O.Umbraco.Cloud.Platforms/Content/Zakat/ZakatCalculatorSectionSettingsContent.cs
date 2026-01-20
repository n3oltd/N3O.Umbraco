using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Zakat.Settings.Calculator.Section.Alias)]
public class ZakatCalculatorSectionSettingsContent : UmbracoContent<ZakatCalculatorSectionSettingsContent> {
    public string Alias => GetAs(x => x.Alias);
    public string Name => Content().Name;
    public new IHtmlEncodedString Content => GetValue(x => x.Content);
    public IEnumerable<ZakatCalculatorFieldSettingsContent> Fields => Content().Children.As<ZakatCalculatorFieldSettingsContent>();
}