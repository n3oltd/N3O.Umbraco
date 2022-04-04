using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Webhooks.Models;

namespace N3O.Umbraco.Webhooks.Extensions {
    public static class WebhookPayloadExtensions {
        public static T GetBody<T>(this WebhookPayload payload, IJsonProvider jsonProvider) {
            if (payload.Body.HasValue()) {
                return jsonProvider.DeserializeObject<T>(payload.Body);
            } else {
                return default;
            }
        }
        
        public static string GetEventId(this WebhookPayload payload) {
            return payload.GetHeader(WebhooksConstants.HttpHeaders.EventId);
        }
        
        public static string GetEventType(this WebhookPayload payload) {
            return payload.GetHeader(WebhooksConstants.HttpHeaders.EventType);
        }
    }
}