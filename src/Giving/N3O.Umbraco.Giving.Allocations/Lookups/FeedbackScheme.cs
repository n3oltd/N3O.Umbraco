using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class FeedbackScheme : ContentOrPublishedLookup, IHoldAllowedGivingTypes, IHoldFundDimensionOptions, IHoldPricing {
    public FeedbackScheme(string id,
                          string name,
                          Guid? contentId,
                          IEnumerable<GivingType> allowedGivingTypes,
                          FundDimensionOptions fundDimensionOptions,
                          Pricing pricing,
                          IEnumerable<FeedbackCustomFieldDefinition> customFields) : base(id, name, contentId) {
        AllowedGivingTypes = allowedGivingTypes;
        FundDimensionOptions = fundDimensionOptions;
        Pricing = pricing;
        CustomFields = customFields;
    }

    public IEnumerable<GivingType> AllowedGivingTypes { get; }
    public FundDimensionOptions FundDimensionOptions { get; }
    public Pricing Pricing { get; }
    public IEnumerable<FeedbackCustomFieldDefinition> CustomFields { get; }

    [JsonIgnore]
    IFundDimensionOptions IHoldFundDimensionOptions.FundDimensionOptions => FundDimensionOptions;
    
    [JsonIgnore]
    IPricing IHoldPricing.Pricing => Pricing;
}