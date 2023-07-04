using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.DirectDebitUK.Models;
using N3O.Umbraco.Payments.NamedParameters;

namespace N3O.Umbraco.Payments.DirectDebitUK.Commands;

public class StoreAccountDetailsCommand : PaymentsCommand<UKBankAccountReq, DirectDebitUKCredential> {
    public StoreAccountDetailsCommand(FlowId flowId) : base(flowId) { }
}