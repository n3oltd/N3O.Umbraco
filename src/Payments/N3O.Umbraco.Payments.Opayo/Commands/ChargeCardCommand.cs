using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Commands {
    public class ChargeCardCommand : PaymentsCommand<ChargeCardReq, OpayoPayment> {
        public ChargeCardCommand(FlowId flowId) : base(flowId) { }
    }
}