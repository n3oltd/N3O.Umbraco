namespace N3O.Umbraco.Payments.DirectDebitUK.Models; 

public interface IUKBankAccount {
    string AccountHolder { get; }
    string AccountNumber { get; }
    string SortCode { get; }
}