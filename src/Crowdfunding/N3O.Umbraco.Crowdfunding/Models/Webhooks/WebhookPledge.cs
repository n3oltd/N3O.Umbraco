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
                         WebhookCrowdfunderInfo crowdfunder,
                         WebhookPledgeTransactions transactions)
        : base(revision, reference) {
        Currency = currency;
        Date = date;
        Account = account;
        Crowdfunder = crowdfunder;
        Transactions = transactions;
    }

    public WebhookCurrency Currency { get; }
    public LocalDate Date { get; }
    public WebhookAccountInfo Account { get; }
    public WebhookCrowdfunderInfo Crowdfunder { get; }
    public WebhookPledgeTransactions Transactions { get; }

    protected override IEnumerable<object> GetValues() {
        yield return Currency;
        yield return Date;
        yield return Account;
        yield return Crowdfunder;
        yield return Transactions;
    }
}