using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class Allocation : Value, IAllocation {
    [JsonConstructor]
    public Allocation(AllocationType type,
                      Money value,
                      FundDimensionValues fundDimensions,
                      FundAllocation fund,
                      SponsorshipAllocation sponsorship,
                      FeedbackAllocation feedback,
                      string pledgeUrl,
                      Guid? upsellOfferId) {
        Type = type;
        Value = value;
        FundDimensions = fundDimensions;
        Fund = fund;
        Sponsorship = sponsorship;
        Feedback = feedback;
        PledgeUrl = pledgeUrl;
        UpsellOfferId = upsellOfferId;
    }

    public Allocation(IAllocation allocation) : this(allocation, allocation.Extensions) { }
    
    public Allocation(IAllocation allocation, IDictionary<string, JToken> extensions)
        : this(allocation.Type,
               allocation.Value,
               allocation.FundDimensions.IfNotNull(x => new FundDimensionValues(x)),
               allocation.Fund.IfNotNull(x => new FundAllocation(x)),
               allocation.Sponsorship.IfNotNull(x => new SponsorshipAllocation(x)),
               allocation.Feedback.IfNotNull(x => new FeedbackAllocation(x)),
               allocation.PledgeUrl,
               allocation.UpsellOfferId) {
        Extensions = extensions;
    }

    public AllocationType Type { get; }
    public Money Value { get; }
    public FundDimensionValues FundDimensions { get; }
    public FundAllocation Fund { get; }
    public SponsorshipAllocation Sponsorship { get; }
    public FeedbackAllocation Feedback { get; }
    public string PledgeUrl { get; }
    public Guid? UpsellOfferId { get; }
    
    public bool LinkedToPledge => PledgeUrl.HasValue();
    
    [JsonExtensionData]
    public IDictionary<string, JToken> Extensions { get; set; }

    [JsonIgnore]
    IFundDimensionValues IAllocation.FundDimensions => FundDimensions;

    [JsonIgnore]
    IFundAllocation IAllocation.Fund => Fund;

    [JsonIgnore]
    ISponsorshipAllocation IAllocation.Sponsorship => Sponsorship;
    
    [JsonIgnore]
    IFeedbackAllocation IAllocation.Feedback => Feedback;

    public string Summary => Fund?.Summary ?? Sponsorship?.Summary ?? Feedback?.Summary;
}
