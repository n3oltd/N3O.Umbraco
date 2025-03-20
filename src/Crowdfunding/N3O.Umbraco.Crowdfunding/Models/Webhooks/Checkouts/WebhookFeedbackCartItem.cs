using N3O.Umbraco.Webhooks.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookFeedbackCartItem : Value {
    public WebhookFeedbackCartItem(WebhookLookup scheme, IEnumerable<WebhookNewCustomField> customFields) {
        Scheme = scheme;
        CustomFields = customFields;
    }

    public WebhookLookup Scheme { get; }
    public IEnumerable<WebhookNewCustomField> CustomFields { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Scheme;
        yield return CustomFields;
    }
}