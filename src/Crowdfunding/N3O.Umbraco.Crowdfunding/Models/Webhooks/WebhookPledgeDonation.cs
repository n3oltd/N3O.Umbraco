using N3O.Umbraco.Webhooks.Models;
using Newtonsoft.Json;
using NodaTime;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookPledgeDonation : Value {
    [JsonConstructor]
    public WebhookPledgeDonation(LocalDate date,
                                 WebhookForexMoney amount,
                                 Guid donationId,
                                 string allocationReference,
                                 bool isImported) {
        Date = date;
        Amount = amount;
        DonationId = donationId;
        AllocationReference = allocationReference;
        IsImported = isImported;
    }
    
    public LocalDate Date { get; }
    public WebhookForexMoney Amount { get; }
    public Guid DonationId { get; }
    public string AllocationReference { get; }
    public bool IsImported { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Date;
        yield return Amount;
        yield return DonationId;
        yield return AllocationReference;
        yield return IsImported;
    }
}