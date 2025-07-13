using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedDonationItem :
    PublishedNamedLookup, IHoldAllowedGivingTypes, IHoldFundDimensionOptions, IHoldPricing {
    public bool AllowOneTime { get; set; }
    public bool AllowRecurring { get; set; }
    public PublishedFundDimensionOptions FundDimensionOptions { get; set; }
    public PublishedPricing Pricing { get; set; }
    
    [JsonIgnore]
    public IEnumerable<GivingType> AllowedGivingTypes {
        get {
            if (AllowOneTime) {
                yield return GivingTypes.Donation;
            }
            
            if (AllowRecurring) {
                yield return GivingTypes.RegularGiving;
            }
        }
    }
    
    [JsonIgnore]
    IFundDimensionOptions IHoldFundDimensionOptions.FundDimensionOptions => FundDimensionOptions;
    
    [JsonIgnore]
    IPricing IHoldPricing.Pricing => Pricing;
}