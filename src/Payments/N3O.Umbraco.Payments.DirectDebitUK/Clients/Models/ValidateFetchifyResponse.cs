namespace N3O.Umbraco.Payments.DirectDebitUK.Clients;

public class ValidateFetchifyResponse {
    public GeneralDetails GeneralDetails { get; set; }
    public string SortCode { get; set; }
    public bool Successfull { get; set; }
    public BankValidate BankValidate { get; set; }
}

public class GeneralDetails {
    public string FullOwningBankName { get; set; }
    public string SubBranchSuffix { get; set; }
    public string LastChangeDate { get; set; }
    public string PrintIndicator { get; set; }
    public string OwningBankCode { get; set; }
    public string ShortBranchTitle { get; set; }
    public string NationalCentralBankCountryCode { get; set; }
    public string ShortOwningBankName { get; set; }
    public string BicBranch { get; set; }
    public string SupervisoryBody { get; set; }
    public string DeletedDate { get; set; }
    public string BicBank { get; set; }
}

public class BankValidate {
    public bool SortCodeListed { get; set; }
    public bool ValidationPerformed { get; set; }
    public bool InvalidAccountNumber { get; set; }
    public string AccountNumber { get; set; }
    public string SortCode { get; set; }
    public bool IsCorrect { get; set; }
}