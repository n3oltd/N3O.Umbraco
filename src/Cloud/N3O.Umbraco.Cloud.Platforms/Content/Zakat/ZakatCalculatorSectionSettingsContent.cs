using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Zakat.Settings.Calculator.Section.Alias)]
public class ZakatCalculatorSectionSettingsContent : UmbracoContent<ZakatCalculatorSectionSettingsContent> {
    public string Alias => GetValue(x => x.Alias);
    public string Name => Content().Name;
    // Either an Umbraco block grid or Perplex blocks depending on the site, so there is no one model behind
    // it; only the alias is used, and IBlocksRenderer handles both
    public new object Content => GetValue(x => x.Content);
    public IReadOnlyList<ZakatCalculatorFieldSettingsContent> Fields =>
        Content().Children.As<ZakatCalculatorFieldSettingsContent>();
}
