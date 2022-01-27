using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Handlers {
    public abstract class PaymentsHandler<TCommand, TReq, TObject> : IRequestHandler<TCommand, TReq, PaymentFlowRes<TObject>>
        where TCommand : PaymentsCommand<TReq, TObject>
        where TObject : PaymentObject, new() {
        private readonly IPaymentsScope _paymentsScope;

        protected PaymentsHandler(IPaymentsScope paymentsScope) {
            _paymentsScope = paymentsScope;
        }
        
        public async Task<PaymentFlowRes<TObject>> Handle(TCommand req, CancellationToken cancellationToken) {
            PaymentFlowRes<TObject> res = null;
            await _paymentsScope.DoAsync<TObject>(async (flow, paymentObject) => {
                await HandleAsync(req, paymentObject, flow, cancellationToken);
                res = new PaymentFlowRes<TObject>(flow, paymentObject);
            }, cancellationToken);
            
            
            return res;
        }

        protected abstract Task HandleAsync(TCommand req,
                                            TObject paymentObject,
                                            IBillingInfoAccessor billingInfoAccessor,
                                            CancellationToken cancellationToken);
    }
}