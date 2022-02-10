using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Commands {
    public class StoreCardCommand : PaymentsCommand<StoreCardReq, OpayoCredential> {
        public StoreCardCommand(FlowId flowId) : base(flowId) { }
    }
}