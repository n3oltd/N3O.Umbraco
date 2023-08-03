using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Giving.Models;

public class Allocation : Value, IAllocation {
    [JsonConstructor]
    public Allocation(AllocationType type,
                      Money value,
                      FundDimensionValues fundDimensions,
                      FundAllocation fund,
                      SponsorshipAllocation sponsorship,
                      FeedbackAllocation feedback,
                      Guid? upsellOfferId) {
        Type = type;
        Value = value;
        FundDimensions = fundDimensions;
        Fund = fund;
        Sponsorship = sponsorship;
        Feedback = feedback;
        UpsellOfferId = upsellOfferId;
    }

    public Allocation(IAllocation allocation)
        : this(allocation.Type,
               allocation.Value,
               allocation.FundDimensions.IfNotNull(x => new FundDimensionValues(x)),
               allocation.Fund.IfNotNull(x => new FundAllocation(x)),
               allocation.Sponsorship.IfNotNull(x => new SponsorshipAllocation(x)),
               allocation.Feedback.IfNotNull(x => new FeedbackAllocation(x)),
               allocation.UpsellOfferId) { }

    public AllocationType Type { get; }
    public Money Value { get; }
    public FundDimensionValues FundDimensions { get; }
    public FundAllocation Fund { get; }
    public SponsorshipAllocation Sponsorship { get; }
    public FeedbackAllocation Feedback { get; }
    public Guid? UpsellOfferId { get; }

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
