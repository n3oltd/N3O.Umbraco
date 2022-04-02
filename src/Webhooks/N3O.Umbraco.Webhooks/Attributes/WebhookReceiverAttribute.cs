using System;

namespace N3O.Umbraco.Webhooks.Attributes {
    public class WebhookReceiverAttribute : Attribute {
        public WebhookReceiverAttribute(string eventId) {
            EventId = eventId;
        }

        public string EventId { get; }
    }
}