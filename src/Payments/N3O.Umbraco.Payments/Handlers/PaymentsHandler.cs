using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Handlers;

public abstract class PaymentsHandler<TCommand, TReq, TObject> :
    IRequestHandler<TCommand, TReq, PaymentFlowRes<TObject>>
    where TCommand : PaymentsCommand<TReq, TObject>
    where TObject : PaymentObject, new() {
    private readonly IPaymentsScope _paymentsScope;

    protected PaymentsHandler(IPaymentsScope paymentsScope) {
        _paymentsScope = paymentsScope;
    }
    
    public async Task<PaymentFlowRes<TObject>> Handle(TCommand req, CancellationToken cancellationToken) {
        var res = await _paymentsScope.DoAsync<TObject>(async (flow, paymentObject) => {
            await HandleAsync(req, paymentObject, new PaymentsParameters(flow), cancellationToken);
        }, cancellationToken);

        return res;
    }

    protected async Task DoAsync<TOtherObject>(Func<TOtherObject, Task> actionAsync,
                                               CancellationToken cancellationToken)
        where TOtherObject : PaymentObject, new() {
        await _paymentsScope.DoAsync<TOtherObject>((_, paymentObject) => actionAsync(paymentObject),
                                                   cancellationToken);
    }

    protected abstract Task HandleAsync(TCommand req,
                                        TObject paymentObject,
                                        PaymentsParameters parameters,
                                        CancellationToken cancellationToken);
}
