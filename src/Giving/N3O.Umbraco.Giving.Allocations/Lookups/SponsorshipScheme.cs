using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class SponsorshipScheme : ContentOrPublishedLookup, IHoldAllowedGivingTypes, IHoldFundDimensionOptions {
    public SponsorshipScheme(string id,
                             string name,
                             Guid? contentId,
                             IEnumerable<GivingType> allowedGivingTypes,
                             IEnumerable<SponsorshipDuration> allowedDurations,
                             FundDimensionOptions fundDimensionOptions,
                             IEnumerable<SponsorshipComponent> components,
                             IEnumerable<string> availableLocations) 
        : base(id, name, contentId) {
        AllowedGivingTypes = allowedGivingTypes;
        AllowedDurations = allowedDurations;
        FundDimensionOptions = fundDimensionOptions;
        Components = components;
        AvailableLocations = availableLocations;
    }

    public IEnumerable<GivingType> AllowedGivingTypes { get; }
    public IEnumerable<SponsorshipDuration> AllowedDurations { get; }
    public FundDimensionOptions FundDimensionOptions { get; }
    public IEnumerable<SponsorshipComponent> Components { get; }
    public IEnumerable<string> AvailableLocations { get; }

    [JsonIgnore]
    IFundDimensionOptions IHoldFundDimensionOptions.FundDimensionOptions => FundDimensionOptions;
}