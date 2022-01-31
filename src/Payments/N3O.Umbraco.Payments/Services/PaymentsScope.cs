using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.NamedParameters;
using System;
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

        public async Task<PaymentFlowRes<T>> DoAsync<T>(Func<IPaymentsFlow, T, Task> actionAsync,
                                                        CancellationToken cancellationToken = default)
            where T : PaymentObject, new() {
            var flow = await _repository.GetAsync(_flowId.Value, cancellationToken);

            var paymentObject = flow.GetOrCreatePaymentObject<T>();

            try {
                await actionAsync(flow, paymentObject);
            } catch (Exception ex) {
                paymentObject.UnhandledError(ex);
            }

            await _repository.UpdateAsync(flow, cancellationToken);

            return new PaymentFlowRes<T>(flow, paymentObject);
        }

        public async Task<T> GetAsync<T>(Func<IPaymentsFlow, T> get, CancellationToken cancellationToken = default) {
            var flow = await _repository.GetAsync(_flowId.Value, cancellationToken);

            return get(flow);
        }
    }
}