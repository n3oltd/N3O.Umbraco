using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class SponsorshipScheme :
    LookupContent<SponsorshipScheme>, IHoldAllowedGivingTypes, IHoldFundDimensionOptions {
    public IEnumerable<GivingType> AllowedGivingTypes => GetValue(x => x.AllowedGivingTypes);
    public IEnumerable<SponsorshipDuration> AllowedDurations => GetValue(x => x.AllowedDurations);

    [UmbracoProperty(AllocationsConstants.Aliases.SponsorshipScheme.Properties.Dimension1)]
    public IEnumerable<FundDimension1Value> Dimension1 => GetPickedAs(x => x.Dimension1);
    
    [UmbracoProperty(AllocationsConstants.Aliases.SponsorshipScheme.Properties.Dimension2)]
    public IEnumerable<FundDimension2Value> Dimension2 => GetPickedAs(x => x.Dimension2);
    
    [UmbracoProperty(AllocationsConstants.Aliases.SponsorshipScheme.Properties.Dimension3)]
    public IEnumerable<FundDimension3Value> Dimension3 => GetPickedAs(x => x.Dimension3);
    
    [UmbracoProperty(AllocationsConstants.Aliases.SponsorshipScheme.Properties.Dimension4)]
    public IEnumerable<FundDimension4Value> Dimension4 => GetPickedAs(x => x.Dimension4);
    
    public IEnumerable<SponsorshipComponent> Components => Content().Children.As<SponsorshipComponent>();

    [JsonIgnore]
    public FundDimensionOptions FundDimensionOptions => new(Dimension1, Dimension2, Dimension3, Dimension4);

    [JsonIgnore]
    IFundDimensionOptions IHoldFundDimensionOptions.FundDimensionOptions => FundDimensionOptions;
}
