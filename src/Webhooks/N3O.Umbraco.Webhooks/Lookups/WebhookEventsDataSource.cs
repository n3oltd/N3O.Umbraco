using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Webhooks.Lookups {
    public class WebhookEventsDataSource : LookupsDataSource<WebhookEvent> {
        public WebhookEventsDataSource(ILookups lookups) : base(lookups) { }
        
        public override string Name => "Webhook Events";
        public override string Description => "Data source for webhook events";
        public override string Icon => "icon-connection";

        protected override string GetDescription(WebhookEvent webhookEvent) => webhookEvent.Description;
        protected override string GetIcon(WebhookEvent webhookEvent) => webhookEvent.Icon;
    }
}