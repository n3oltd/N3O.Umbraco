using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IAllocation {
    AllocationType Type { get; }
    Money Value { get; }
    IFundDimensionValues FundDimensions { get; }
    IFundAllocation Fund { get; }
    ISponsorshipAllocation Sponsorship { get; }
    IFeedbackAllocation Feedback { get; }
    string PledgeUrl { get; }
    bool LinkedToPledge { get; }
    Guid? UpsellOfferId { get; }
    string Notes { get; }
    IDictionary<string, JToken> Extensions { get; }
}