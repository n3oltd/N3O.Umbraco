namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public class TotalProcessingApiSettings : Value {
    public TotalProcessingApiSettings(string baseUrl, string accessToken, string entityId) {
        BaseUrl = baseUrl;
        AccessToken = accessToken;
        EntityId = entityId;
    }

    public string BaseUrl { get; }
    public string AccessToken { get; }
    public string EntityId { get; }
}
