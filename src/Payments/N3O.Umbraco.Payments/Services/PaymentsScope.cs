using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.NamedParameters;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments {
    public class PaymentsScope : IPaymentsScope {
        private readonly IRepository<IPaymentsFlow> _repository;
        private readonly FlowId _flowId;

        public PaymentsScope(IRepository<IPaymentsFlow> repository, FlowId flowId) {
            _repository = repository;
            _flowId = flowId;
        }

        public async Task DoAsync<TPaymentObject>(Func<IPaymentsFlow, TPaymentObject, Task> actionAsync,
                                                  CancellationToken cancellationToken = default)
            where TPaymentObject : PaymentObject, new() {
            var flow = await _repository.GetAsync(_flowId.Value, cancellationToken);

            // TODO Need to check if for example payment has already been taken or credential set-up that
            // we don't end up repeating that process

            var paymentObject = flow.PaymentObjects.OfType<TPaymentObject>()
                                    .SingleOrDefault();

            if (paymentObject == null) {
                paymentObject = new TPaymentObject();

                flow.AddOrReplacePaymentObject(paymentObject);
            }

            try {
                await actionAsync(flow, paymentObject);

                await _repository.UpdateAsync(flow, cancellationToken);
            } catch (Exception ex) {
                await flow.OnErrorAsync(paymentObject, ex, cancellationToken);
            }
        }

        public async Task<T> GetAsync<T>(Func<IPaymentsFlow, T> get, CancellationToken cancellationToken = default) {
            var flow = await _repository.GetAsync(_flowId.Value, cancellationToken);

            return get(flow);
        }
    }
}