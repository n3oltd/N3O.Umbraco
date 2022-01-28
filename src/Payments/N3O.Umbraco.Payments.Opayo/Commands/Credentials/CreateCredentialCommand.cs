using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Commands {
    public class CreateCredentialCommand : PaymentsCommand<OpayoCredentialReq, OpayoCredential> {
        public CreateCredentialCommand(FlowId flowId) : base(flowId) { }
    }
}