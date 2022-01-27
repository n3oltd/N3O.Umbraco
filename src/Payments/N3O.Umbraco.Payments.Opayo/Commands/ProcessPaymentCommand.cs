using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Commands {
    public class ProcessPaymentCommand : PaymentsCommand<OpayoPaymentReq, OpayoPayment> {
        public ProcessPaymentCommand(FlowId flowId) : base(flowId) { }
    }
}