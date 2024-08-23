using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.FundraiserAllocation.Alias)]
public class FundraiserAllocationElement : UmbracoElement<FundraiserAllocationElement>, IFundDimensionValues {
    public string Title => GetValue(x => x.Title);
    public decimal Amount => GetValue(x => x.Amount);
    public FundDimension1Value FundDimension1 => GetAs(x => x.FundDimension1);
    public FundDimension2Value FundDimension2 => GetAs(x => x.FundDimension2);
    public FundDimension3Value FundDimension3 => GetAs(x => x.FundDimension3);
    public FundDimension4Value FundDimension4 => GetAs(x => x.FundDimension4);
    public IEnumerable<PriceHandleElement> PriceHandles => GetNestedAs(x => x.PriceHandles);

    public override void Content(IPublishedElement content) {
        base.Content(content);
        
        if (Type == AllocationTypes.Fund) {
            Fund = new FundFundraiserAllocationElement();
            Fund.Content(content);
        } else if (Type == AllocationTypes.Feedback) {
            Feedback = new FeedbackFundraiserAllocationElement();
            Feedback.Content(content);
        } else if (Type == AllocationTypes.Sponsorship) {
            Sponsorship = new SponsorshipFundraiserAllocationElement();
            Sponsorship.Content(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public FundFundraiserAllocationElement Fund { get; private set; }
    public FeedbackFundraiserAllocationElement Feedback { get; private set; }
    public SponsorshipFundraiserAllocationElement Sponsorship { get; private set; }

    public IFundDimensionsOptions GetFundDimensionOptions() {
        return (IFundDimensionsOptions) Fund?.DonationItem ??
               (IFundDimensionsOptions) Sponsorship?.Scheme ??
               (IFundDimensionsOptions) Feedback?.Scheme;
    }
    
    [JsonIgnore]
    public AllocationType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(CrowdfundingConstants.FundraiserAllocation.Fund.Alias)) {
                return AllocationTypes.Fund;
            } else if (Content().ContentType.Alias.EqualsInvariant(CrowdfundingConstants.FundraiserAllocation.Feedback.Alias)) {
                return AllocationTypes.Feedback;
            } else if (Content().ContentType.Alias.EqualsInvariant(CrowdfundingConstants.FundraiserAllocation.Sponsorship.Alias)) {
                return AllocationTypes.Sponsorship;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
    
    [JsonIgnore]
    FundDimension1Value IFundDimensionValues.Dimension1 => FundDimension1;
    
    [JsonIgnore]
    FundDimension2Value IFundDimensionValues.Dimension2 => FundDimension2;
    
    [JsonIgnore]
    FundDimension3Value IFundDimensionValues.Dimension3 => FundDimension3;
    
    [JsonIgnore]
    FundDimension4Value IFundDimensionValues.Dimension4 => FundDimension4;
}
