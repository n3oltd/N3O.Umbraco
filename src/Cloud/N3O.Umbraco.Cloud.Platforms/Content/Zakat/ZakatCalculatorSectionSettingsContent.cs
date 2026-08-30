using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Zakat.Settings.Calculator.Section.Alias)]
public class ZakatCalculatorSectionSettingsContent : UmbracoContent<ZakatCalculatorSectionSettingsContent> {
    public string Alias => GetValue(x => x.Alias);
    public string Name => Content().Name;
    // A site renders page content with either Umbraco block grids or Perplex content blocks, so there is
    // no one model behind this property. Only its presence is read here and it is rendered through
    // IBlocksRenderer by alias, which handles both
    public new object Content => GetValue(x => x.Content);
    public IEnumerable<ZakatCalculatorFieldSettingsContent> Fields => Content().Children.As<ZakatCalculatorFieldSettingsContent>();
}