using N3O.Umbraco.Webhooks.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookCheckout : WebhookEntity {
    [JsonConstructor]
    public WebhookCheckout(WebhookRevision revision,
                           WebhookReference reference,
                           WebhookRevision cartRevisionId,
                           WebhookCurrency currency,
                           WebhookCheckoutProgress progress,
                           WebhookCheckoutAccountInfo account,
                           WebhookCheckoutDonation donation,
                           IPAddress remoteIp)
        : base(revision, reference) {
        CartRevisionId = cartRevisionId;
        Currency = currency;
        Progress = progress;
        Account = account;
        Donation = donation;
        RemoteIp = remoteIp;
    }
    
    public WebhookRevision CartRevisionId { get; }
    public WebhookCurrency Currency { get; }
    public WebhookCheckoutProgress Progress { get; }
    public WebhookCheckoutAccountInfo Account { get; }
    public WebhookCheckoutDonation Donation { get; }
    public IPAddress RemoteIp { get; }

    protected override IEnumerable<object> GetValues() {
        yield return CartRevisionId;
        yield return Currency;
        yield return Progress;
        yield return Account;
        yield return Donation;
        yield return RemoteIp;
    }
}