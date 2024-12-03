using N3O.Umbraco.Extensions;
using N3O.Umbraco.Elements.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Lookups;

public class SponsorshipScheme :
    LookupContent<SponsorshipScheme>, IFundDimensionsOptions, IHoldAllowedGivingTypes {
    public IEnumerable<GivingType> AllowedGivingTypes => GetValue(x => x.AllowedGivingTypes);
    public IEnumerable<SponsorshipDuration> AllowedDurations => GetValue(x => x.AllowedDurations);
    public IEnumerable<FundDimension1Value> Dimension1Options => GetPickedAs(x => x.Dimension1Options);
    public IEnumerable<FundDimension2Value> Dimension2Options => GetPickedAs(x => x.Dimension2Options);
    public IEnumerable<FundDimension3Value> Dimension3Options => GetPickedAs(x => x.Dimension3Options);
    public IEnumerable<FundDimension4Value> Dimension4Options => GetPickedAs(x => x.Dimension4Options);
    public IEnumerable<SponsorshipComponent> Components => Content().Children.As<SponsorshipComponent>();
}
