namespace N3O.Umbraco.Webhooks;

public static class WebhooksConstants {
    public const string ApiName = "Webhooks";
    
    public class HttpHeaders {
        public static readonly string EventId = "N3O-Event-Id";
        public static readonly string EventType = "N3O-Event-Type";
    }
}
