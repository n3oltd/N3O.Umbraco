using N3O.Umbraco.Payments.DirectDebitUK.Commands;
using N3O.Umbraco.Payments.DirectDebitUK.Models;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.DirectDebitUK.Handlers;

public class StoreAccountDetailsHandler :
    PaymentsHandler<StoreAccountDetailsCommand, UKBankAccountReq, DirectDebitUKCredential> {
    public StoreAccountDetailsHandler(IPaymentsScope paymentsScope) : base(paymentsScope) { }

    protected override Task HandleAsync(StoreAccountDetailsCommand req,
                                        DirectDebitUKCredential credential,
                                        PaymentsParameters parameters,
                                        CancellationToken cancellationToken) {
        credential.StoreAccountDetails(req.Model);

        return Task.CompletedTask;
    }
}