using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Webhooks.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookCartItem : Value {
    public WebhookCartItem(WebhookLookup type, WebhookMoney value,
                           WebhookFundDimensionValues fundDimensions,
                           WebhookFundCartItem fund,
                           WebhookFeedbackCartItem feedback,
                           Dictionary<string, JToken> extensions) {
        Type = type;
        Value = value;
        FundDimensions = fundDimensions;
        Fund = fund;
        Feedback = feedback;
        Extensions = extensions;
    }

    public WebhookLookup Type { get; }
    public WebhookMoney Value { get; }
    public WebhookFundDimensionValues FundDimensions { get; }
    public WebhookFundCartItem Fund { get; }
    public WebhookFeedbackCartItem Feedback { get; }
    public Dictionary<string, JToken> Extensions { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Type;
        yield return Value;
        yield return Extensions;
    }

    public Allocation ToAllocation(ILookups lookups) {
        var allocationType = lookups.FindById<AllocationType>(Type.Id);

        var allocation = new Allocation(allocationType,
                                        Value.ToMoney(lookups),
                                        GetFundDimensionValues(lookups, FundDimensions),
                                        Fund.ToFundAllocation(lookups),
                                        null,
                                        Feedback.ToFeedbackAllocation(lookups),
                                        null,
                                        null);
        
        allocation = new Allocation(allocation, Extensions);

        return allocation;
    }
    
    // TODO Move
    private static FundDimensionValues GetFundDimensionValues(ILookups lookups,
                                                              WebhookFundDimensionValues webhookFundDimensionValues) {
        var dimension1Value = webhookFundDimensionValues.Dimension1.IfNotNull(lookups.FindByName<FundDimension1Value>)
                                                        .SingleOrDefault();

        var dimension2Value = webhookFundDimensionValues.Dimension2.IfNotNull(lookups.FindByName<FundDimension2Value>)
                                                        .SingleOrDefault();

        var dimension3Value = webhookFundDimensionValues.Dimension3.IfNotNull(lookups.FindByName<FundDimension3Value>)
                                                        .SingleOrDefault();

        var dimension4Value = webhookFundDimensionValues.Dimension4.IfNotNull(lookups.FindByName<FundDimension4Value>)
                                                        .SingleOrDefault();

        var fundDimensionValues =
            new FundDimensionValues(dimension1Value, dimension2Value, dimension3Value, dimension4Value);

        return fundDimensionValues;
    }
}
