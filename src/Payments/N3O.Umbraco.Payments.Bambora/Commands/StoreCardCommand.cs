using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;

namespace N3O.Umbraco.Payments.Bambora.Commands {
    public class StoreCardCommand : PaymentsCommand<StoreCardReq, BamboraCredential> {
        public StoreCardCommand(FlowId flowId) : base(flowId) { }
    }
}