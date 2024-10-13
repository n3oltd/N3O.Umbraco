using N3O.Umbraco.Webhooks.Models;
using Newtonsoft.Json;
using NodaTime;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookPledgeDonation : Value {
    public WebhookPledgeDonation(LocalDate date,
                                 WebhookForexMoney amount,
                                 Guid donationId,
                                 WebhookReference accountReference,
                                 string accountName,
                                 string accountEmail,
                                 string allocationReference,
                                 WebhookFundDimensionValues fundDimensionValues,
                                 string summary,
                                 bool isImported) {
        Date = date;
        Amount = amount;
        DonationId = donationId;
        AccountReference = accountReference;
        AccountName = accountName;
        AccountEmail = accountEmail;
        AllocationReference = allocationReference;
        FundDimensionValues = fundDimensionValues;
        Summary = summary;
        IsImported = isImported;
    }
    
    public LocalDate Date { get; }
    public WebhookForexMoney Amount { get; }
    public Guid DonationId { get; }
    public WebhookReference AccountReference { get; }
    public string AccountName { get; }
    public string AccountEmail { get; }
    public string AllocationReference { get; }
    public WebhookFundDimensionValues FundDimensionValues { get; }
    public string Summary { get; }
    public bool IsImported { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Date;
        yield return Amount;
        yield return DonationId;
        yield return AccountReference;
        yield return AccountName;
        yield return AccountEmail;
        yield return AllocationReference;
        yield return FundDimensionValues;
        yield return Summary;
        yield return IsImported;
    }
}