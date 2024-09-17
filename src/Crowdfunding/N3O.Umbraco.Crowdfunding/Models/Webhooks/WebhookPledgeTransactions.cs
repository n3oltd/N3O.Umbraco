using N3O.Umbraco.Webhooks.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookPledgeTransactions : Value {
    public WebhookPledgeTransactions(WebhookMoney nonDonationsBalance,
                                     int importedDonationsCount,
                                     WebhookMoney importedDonationsTotal,
                                     int otherDonationsCount,
                                     WebhookMoney otherDonationsTotal) {
        NonDonationsBalance = nonDonationsBalance;
        ImportedDonationsCount = importedDonationsCount;
        ImportedDonationsTotal = importedDonationsTotal;
        OtherDonationsCount = otherDonationsCount;
        OtherDonationsTotal = otherDonationsTotal;
    }

    public WebhookMoney NonDonationsBalance { get; }
    public int ImportedDonationsCount { get; }
    public WebhookMoney ImportedDonationsTotal { get; }
    public int OtherDonationsCount { get; }
    public WebhookMoney OtherDonationsTotal { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return NonDonationsBalance;
        yield return ImportedDonationsCount;
        yield return ImportedDonationsTotal;
        yield return OtherDonationsCount;
        yield return OtherDonationsTotal;
    }
}