using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Webhooks.Lookups {
    public class WebhookEvent : NamedLookup {
        public WebhookEvent(string id, string name, string description, string icon) : base(id, name) {
            Description = description;
            Icon = icon;
        }
        
        public string Description { get; }
        public string Icon { get; }
    }

    public interface IWebhookEvents { }

    public class WebhookEvents : DistributedLookupsCollection<WebhookEvent, IWebhookEvents> { }
}