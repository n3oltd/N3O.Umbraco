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

public abstract class CrowdfunderGoalElement : UmbracoElement<CrowdfunderGoalElement>, IFundDimensionValues {
    public string Title => GetValue(x => x.Title);
    public decimal Amount => GetValue(x => x.Amount);
    public FundDimension1Value FundDimension1 => GetAs(x => x.FundDimension1);
    public FundDimension2Value FundDimension2 => GetAs(x => x.FundDimension2);
    public FundDimension3Value FundDimension3 => GetAs(x => x.FundDimension3);
    public FundDimension4Value FundDimension4 => GetAs(x => x.FundDimension4);
    public IEnumerable<IPublishedContent> Tags => GetPickedAs(x => x.Tags);
    public IEnumerable<PriceHandleElement> PriceHandles => GetNestedAs(x => x.PriceHandles);
    
    public CrowdfunderFundGoalElement Fund { get; protected set; }
    public CrowdfunderFeedbackGoalElement Feedback { get; protected set; }
    
    public IFundDimensionsOptions GetFundDimensionOptions() {
        return (IFundDimensionsOptions) Fund?.DonationItem ??
               (IFundDimensionsOptions) Feedback?.Scheme;
    }
    
    [JsonIgnore]
    public AllocationType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(FundContentTypeAlias)) {
                return AllocationTypes.Fund;
            } else if (Content().ContentType.Alias.EqualsInvariant(FeedbackContentTypeAlias)) {
                return AllocationTypes.Feedback;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }
    
    public abstract string FundContentTypeAlias { get; }
    public abstract string FeedbackContentTypeAlias { get; }
    public abstract string CampaignGoalId { get; }

    [JsonIgnore]
    FundDimension1Value IFundDimensionValues.Dimension1 => FundDimension1;
    
    [JsonIgnore]
    FundDimension2Value IFundDimensionValues.Dimension2 => FundDimension2;
    
    [JsonIgnore]
    FundDimension3Value IFundDimensionValues.Dimension3 => FundDimension3;
    
    [JsonIgnore]
    FundDimension4Value IFundDimensionValues.Dimension4 => FundDimension4;
}