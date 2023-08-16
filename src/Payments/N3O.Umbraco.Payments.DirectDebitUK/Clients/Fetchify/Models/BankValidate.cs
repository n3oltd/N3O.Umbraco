namespace N3O.Umbraco.Payments.DirectDebitUK.Clients.Fetchify;

public class BankValidate {
    public bool SortCodeListed { get; set; }
    public bool ValidationPerformed { get; set; }
    public bool InvalidAccountNumber { get; set; }
    public string AccountNumber { get; set; }
    public string SortCode { get; set; }
    public bool IsCorrect { get; set; }
}