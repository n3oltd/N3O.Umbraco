using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Webhooks.Models;
using System.Collections.Generic;

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
            return GetGetHeader(payload, WebhooksConstants.HttpHeaders.EventId);
        }
        
        public static string GetEventType(this WebhookPayload payload) {
            return GetGetHeader(payload, WebhooksConstants.HttpHeaders.EventId);
        }
        
        public static string GetGetHeader(WebhookPayload payload, string headerName) {
            return payload.HeaderData?.GetValueOrDefault(headerName);
        }
    }
}