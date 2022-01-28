using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Testing {
    public class TestPaymentScope : IPaymentsScope {
        private readonly IPaymentsFlow _paymentsFlow;

        public TestPaymentScope(IPaymentsFlow paymentsFlow) {
            _paymentsFlow = paymentsFlow;
        }
        public async Task DoAsync<T>(Func<IPaymentsFlow, T, Task> actionAsync,
                                     CancellationToken cancellationToken = default)
            where T : PaymentObject, new() {
            var paymentObject = _paymentsFlow.GetOrCreatePaymentObject<T>();

            try {
                await actionAsync(_paymentsFlow, paymentObject);
            } catch (Exception ex) {
                paymentObject.UnhandledError(ex);
            }
        }

        public Task<T> GetAsync<T>(Func<IPaymentsFlow, T> get, CancellationToken cancellationToken = default) {
            return Task.FromResult(get(_paymentsFlow));
        }
    }
}