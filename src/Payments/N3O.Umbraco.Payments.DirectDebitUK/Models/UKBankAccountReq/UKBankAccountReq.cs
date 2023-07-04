using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.DirectDebitUK.Models;

public class UKBankAccountReq : IUKBankAccount {
    [Name("Account Holder")]
    public string AccountHolder { get; }
    
    [Name("Account Number")]
    public string AccountNumber { get; }
    
    [Name("Sort Code")]
    public string SortCode { get; }
}