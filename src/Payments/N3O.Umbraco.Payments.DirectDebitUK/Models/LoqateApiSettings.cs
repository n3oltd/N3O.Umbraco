namespace N3O.Umbraco.Payments.DirectDebitUK.Models; 

public class LoqateApiSettings : Value {
    public LoqateApiSettings(string apiKey) {
        ApiKey = apiKey;
    }
    
    public string ApiKey { get; }
}