using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Testing {
    public class TestPaymentScope : IPaymentsScope {
        private readonly TestPaymentsFlow _testPaymentsFlow;

        public TestPaymentScope(TestPaymentsFlow testPaymentsFlow) {
            _testPaymentsFlow = testPaymentsFlow;
        }
        public async Task DoAsync<T>(Func<IPaymentsFlow, T, Task> actionAsync,
                                     CancellationToken cancellationToken = default)
            where T : PaymentObject, new() {
            var paymentObject = _testPaymentsFlow.GetOrCreatePaymentObject<T>();

            try {
                await actionAsync(_testPaymentsFlow, paymentObject);
            } catch (Exception ex) {
                paymentObject.UnhandledError(ex);
            }
        }

        public async Task<T> GetAsync<T>(Func<IPaymentsFlow, T> get, CancellationToken cancellationToken = default) {
            return get(_testPaymentsFlow);
        }
    }
}