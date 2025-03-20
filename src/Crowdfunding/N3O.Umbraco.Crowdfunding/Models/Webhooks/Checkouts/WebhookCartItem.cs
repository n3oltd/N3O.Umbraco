using N3O.Umbraco.Webhooks.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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

    public string GetSummary() => Fund?.DonationItem?.Name ?? Feedback?.Scheme?.Name;
}
