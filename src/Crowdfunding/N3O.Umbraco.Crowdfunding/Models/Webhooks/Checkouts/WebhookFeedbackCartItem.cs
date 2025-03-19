using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Webhooks.Models;
using System.Collections.Generic;
using System.Linq;

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

    public FeedbackAllocation ToFeedbackAllocation(ILookups lookups) {
        var feedbackScheme = lookups.FindById<FeedbackScheme>(Scheme.Id);
        // TODO
        return new FeedbackAllocation(feedbackScheme, null);
    }
}