using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Bambora.Models;

namespace N3O.Umbraco.Payments.Bambora.Commands {
    public class ChargeCardCommand : PaymentsCommand<ChargeCardReq, BamboraPayment> {
        public ChargeCardCommand(FlowId flowId) : base(flowId) { }
    }
}