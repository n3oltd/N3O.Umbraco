using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Zakat.Settings.Calculator.Alias)]
public class ZakatCalculatorSettingsContent : UmbracoContent<ZakatCalculatorSettingsContent> {
    public OfferingContent Offering => GetPickedAs<OfferingContent>(x => x.Offering);
    public FundDimension1 FundDimension1 => GetValue(x => x.FundDimension1);
    public FundDimension1 FundDimension2 => GetValue(x => x.FundDimension2);
    public FundDimension1 FundDimension3 => GetValue(x => x.FundDimension3);
    public FundDimension1 FundDimension4 => GetValue(x => x.FundDimension4);
    public IEnumerable<ZakatCalculatorSectionSettingsContent> Sections => Content().Children.As<ZakatCalculatorSectionSettingsContent>();
}