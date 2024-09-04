using N3O.Umbraco.Webhooks.Models;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookPledge : WebhookEntity {
    public WebhookPledge(WebhookRevision revision,
                         WebhookReference reference,
                         WebhookCurrency currency,
                         LocalDate date,
                         WebhookAccountInfo account,
                         WebhookCrowdfundingInfo crowdfunding,
                         WebhookPledgeBalanceSummary balanceSummary)
        : base(revision, reference) {
        Currency = currency;
        Date = date;
        Account = account;
        Crowdfunding = crowdfunding;
        BalanceSummary = balanceSummary;
    }

    public WebhookCurrency Currency { get; }
    public LocalDate Date { get; }
    public WebhookAccountInfo Account { get; }
    public WebhookCrowdfundingInfo Crowdfunding { get; }
    public WebhookPledgeBalanceSummary BalanceSummary { get; }

    protected override IEnumerable<object> GetValues() {
        yield return Currency;
        yield return Date;
        yield return Account;
    }
}