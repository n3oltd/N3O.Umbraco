using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.DirectDebitUK.Models;

public class UKBankAccountReq : IUKBankAccount {
    [Name("Account Holder")]
    public string AccountHolder { get; set; }
    
    [Name("Account Number")]
    public string AccountNumber { get; set; }
    
    [Name("Sort Code")]
    public string SortCode { get; set; }
}