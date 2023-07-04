using System.Collections.Generic;

namespace N3O.Umbraco.Payments.DirectDebitUK.Clients;

public class ValidateResponse {
    public IEnumerable<ValidateResult> Items { get; set; }
}

public class ValidateResult {
    public bool IsCorrect { get; set; }
    public bool IsDirectDebitCapable { get; set; }
    public bool FasterPaymentsSupported { get; set; }
    public bool CHAPSSupported { get; set; }
    public string StatusInformation { get; set; }
    public string CorrectedSortCode { get; set; }
    public string CorrectedAccountNumber { get; set; }
    public string IBAN { get; set; }
    public string Bank { get; set; }
    public string BankBIC { get; set; }
    public string Branch { get; set; }
    public string BranchBIC { get; set; }
    public string ContactAddressLine1 { get; set; }
    public string ContactAddressLine2 { get; set; }
    public string ContactPostTown { get; set; }
    public string ContactPostcode { get; set; }
    public string ContactPhone { get; set; }
    public string ContactFax { get; set; }
    
    public string Error { get; set; }
    public string Description { get; set; }
    public string Cause { get; set; }
    public string Resolution { get; set; }
}
