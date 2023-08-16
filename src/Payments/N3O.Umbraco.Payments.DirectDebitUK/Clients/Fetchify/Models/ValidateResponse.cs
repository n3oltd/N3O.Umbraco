namespace N3O.Umbraco.Payments.DirectDebitUK.Clients.Fetchify; 

public class ValidateResponse {
    public GeneralDetails General { get; set; }
    public string SortCode { get; set; }
    public bool Successful { get; set; }
    public BankValidate BankValidate { get; set; }
}