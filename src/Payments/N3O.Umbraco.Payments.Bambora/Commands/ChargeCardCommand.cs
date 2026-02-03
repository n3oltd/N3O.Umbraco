using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;

namespace N3O.Umbraco.Payments.Bambora.Commands;

public class ChargeCardCommand : PaymentsCommand<ChargeCardReq, BamboraPayment> {
    public ChargeCardCommand(FlowId flowId) : base(flowId) { }
}
