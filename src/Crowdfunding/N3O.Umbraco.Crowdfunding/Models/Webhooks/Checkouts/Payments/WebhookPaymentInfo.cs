using N3O.Umbraco.Webhooks.Models;
using NodaTime;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookPaymentInfo : Value {
    public WebhookPaymentInfo(Guid id,
                              WebhookLookup initiatedBy,
                              WebhookReference reference,
                              LocalDate date,
                              LocalDate? paidOutOn,
                              WebhookCurrency currency,
                              WebhookForexMoney value,
                              WebhookForexMoney refundedValue,
                              WebhookForexMoney balance,
                              WebhookLookup method,
                              string summary,
                              string receiptBookNumber,
                              string receivedBy,
                              string customField1,
                              string customField2,
                              string customField3,
                              string customField4,
                              string transactionDetails1,
                              string transactionDetails2,
                              string transactionDetails3,
                              string transactionDetails4,
                              string transactionDetails5,
                              string transactionDetails6,
                              WebhookLookup status) {
        Id = id;
        InitiatedBy = initiatedBy;
        Reference = reference;
        Date = date;
        PaidOutOn = paidOutOn;
        Currency = currency;
        Value = value;
        RefundedValue = refundedValue;
        Balance = balance;
        Method = method;
        Summary = summary;
        ReceiptBookNumber = receiptBookNumber;
        ReceivedBy = receivedBy;
        CustomField1 = customField1;
        CustomField2 = customField2;
        CustomField3 = customField3;
        CustomField4 = customField4;
        TransactionDetails1 = transactionDetails1;
        TransactionDetails2 = transactionDetails2;
        TransactionDetails3 = transactionDetails3;
        TransactionDetails4 = transactionDetails4;
        TransactionDetails5 = transactionDetails5;
        TransactionDetails6 = transactionDetails6;
        Status = status;
    }

    public Guid Id { get; }
    public WebhookLookup InitiatedBy { get; }
    public WebhookReference Reference { get; }
    public LocalDate Date { get; }
    public LocalDate? PaidOutOn { get; }
    public WebhookCurrency Currency { get; }
    public WebhookForexMoney Value { get; }
    public WebhookForexMoney RefundedValue { get; }
    public WebhookForexMoney Balance { get; }
    public WebhookLookup Method { get; }
    public string Summary { get; }
    public string ReceiptBookNumber { get; }
    public string ReceivedBy { get; }
    public string CustomField1 { get; }
    public string CustomField2 { get; }
    public string CustomField3 { get; }
    public string CustomField4 { get; }
    public string TransactionDetails1 { get; }
    public string TransactionDetails2 { get; }
    public string TransactionDetails3 { get; }
    public string TransactionDetails4 { get; }
    public string TransactionDetails5 { get; }
    public string TransactionDetails6 { get; }
    public WebhookLookup Status { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return InitiatedBy;
        yield return Reference;
        yield return Date;
        yield return PaidOutOn;
        yield return Currency;
        yield return Value;
        yield return RefundedValue;
        yield return Balance;
        yield return Method;
        yield return Summary;
        yield return ReceiptBookNumber;
        yield return ReceivedBy;
        yield return CustomField1;
        yield return CustomField2;
        yield return CustomField3;
        yield return CustomField4;
        yield return TransactionDetails1;
        yield return TransactionDetails2;
        yield return TransactionDetails3;
        yield return TransactionDetails4;
        yield return TransactionDetails5;
        yield return TransactionDetails6;
        yield return Status;
    }
}