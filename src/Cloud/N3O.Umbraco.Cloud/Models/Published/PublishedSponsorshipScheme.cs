using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedSponsorshipScheme : PublishedNamedLookup, IHoldAllowedGivingTypes, IHoldFundDimensionOptions {
    public PublishedFundDimensionOptions FundDimensionOptions { get; set; }
    public IEnumerable<PublishedSponsorshipComponent> Components { get; set; }
    public IEnumerable<PublishedCommitmentDuration> AllowedDurations { get; set; }
    
    [JsonIgnore]
    public IEnumerable<GivingType> AllowedGivingTypes {
        get {
            yield return GivingTypes.Donation;
            yield return GivingTypes.RegularGiving;
        }
    }

    [JsonIgnore]
    IFundDimensionOptions IHoldFundDimensionOptions.FundDimensionOptions => FundDimensionOptions;
}