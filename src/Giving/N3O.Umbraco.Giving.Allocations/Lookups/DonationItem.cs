using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class DonationItem : ContentOrPublishedLookup, IHoldAllowedGivingTypes, IHoldFundDimensionOptions, IHoldPricing {
    public DonationItem(string id,
                        string name,
                        Guid? contentId,
                        IEnumerable<GivingType> allowedGivingTypes,
                        FundDimensionOptions fundDimensionOptions,
                        Pricing pricing)
        : base(id, name, contentId) {
        AllowedGivingTypes = allowedGivingTypes;
        FundDimensionOptions = fundDimensionOptions;
        Pricing = pricing;
    }

    public IEnumerable<GivingType> AllowedGivingTypes { get; }
    public FundDimensionOptions FundDimensionOptions { get; }
    public Pricing Pricing { get; }

    [JsonIgnore]
    IFundDimensionOptions IHoldFundDimensionOptions.FundDimensionOptions => FundDimensionOptions;
    
    [JsonIgnore]
    IPricing IHoldPricing.Pricing => Pricing;
}