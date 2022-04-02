using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Webhooks.NamedParameters {
    public class WebhookRoute : NamedParameter<string> {
        public override string Name => "webhookRoute";
    }
}