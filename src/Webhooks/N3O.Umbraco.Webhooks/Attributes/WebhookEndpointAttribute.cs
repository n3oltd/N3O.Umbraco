using System;

namespace N3O.Umbraco.Webhooks.Attributes {
    public class WebhookEndpointAttribute : Attribute {
        public WebhookEndpointAttribute(string endpointId) {
            EndpointId = endpointId;
        }

        public string EndpointId { get; }
    }
}