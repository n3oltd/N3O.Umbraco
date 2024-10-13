using N3O.Umbraco.Webhooks.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookPledgeTransactions : Value {
    public WebhookPledgeTransactions(WebhookMoney nonDonationsBalance, IEnumerable<WebhookPledgeDonation> donations) {
        NonDonationsBalance = nonDonationsBalance;
        Donations = donations;
    }

    public WebhookMoney NonDonationsBalance { get; }
    public IEnumerable<WebhookPledgeDonation> Donations { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return NonDonationsBalance;
        yield return Donations;
    }
}