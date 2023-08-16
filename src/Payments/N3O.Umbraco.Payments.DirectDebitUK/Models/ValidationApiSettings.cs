namespace N3O.Umbraco.Payments.DirectDebitUK.Models; 

public class ValidationApiSettings : Value {
    public ValidationApiSettings(string fetchifyApiKey, string loqateApiKey) {
        FetchifyApiKey = fetchifyApiKey;
        LoqateApiKey = loqateApiKey;
    }
    
    public string FetchifyApiKey { get; }
    public string LoqateApiKey { get; }
}