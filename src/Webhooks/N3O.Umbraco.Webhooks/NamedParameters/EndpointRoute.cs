using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Webhooks.NamedParameters {
    public class EndpointRoute : NamedParameter<string> {
        public override string Name => "endpointRoute";
    }
}