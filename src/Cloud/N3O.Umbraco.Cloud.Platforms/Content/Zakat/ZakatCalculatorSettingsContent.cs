using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.Blocks;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Zakat.Settings.Calculator.Alias)]
public class ZakatCalculatorSettingsContent : UmbracoContent<ZakatCalculatorSettingsContent> {
    public BlockGridModel DefaultContent => GetValue(x => x.DefaultContent);
    public string EmailCompositionId => GetValue(x => x.EmailCompositionId);
    public NisabType DefaultNisabType => GetValue(x => x.DefaultNisabType);
    public OfferingContent Offering => GetPickedAs(x => x.Offering);
    public FundDimension1 FundDimension1 => GetValue(x => x.FundDimension1);
    public FundDimension1 FundDimension2 => GetValue(x => x.FundDimension2);
    public FundDimension1 FundDimension3 => GetValue(x => x.FundDimension3);
    public FundDimension1 FundDimension4 => GetValue(x => x.FundDimension4);
    public IEnumerable<ZakatCalculatorSectionSettingsContent> Sections => Content().Children.As<ZakatCalculatorSectionSettingsContent>();
}