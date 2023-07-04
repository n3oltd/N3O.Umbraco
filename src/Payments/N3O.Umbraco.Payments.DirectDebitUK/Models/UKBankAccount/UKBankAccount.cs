using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.DirectDebitUK.Models; 

public class UKBankAccount : Value, IUKBankAccount {
    [JsonConstructor]
    public UKBankAccount(string accountHolder, string accountNumber, string sortCode) {
        AccountHolder = accountHolder;
        AccountNumber = accountNumber;
        SortCode = sortCode;
    }

    public UKBankAccount(IUKBankAccount bankAccount)
        : this(bankAccount.AccountHolder, bankAccount.AccountNumber, bankAccount.SortCode) { }

    public string AccountHolder { get; }
    public string AccountNumber { get; }
    public string SortCode { get; }
}