using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Testing {
    public class TestPaymentsScope : PaymentsScopeBase {
        private IPaymentsFlow _paymentsFlow;

        public TestPaymentsScope(ILookups lookups) {
            _paymentsFlow = new TestPaymentsFlow(lookups);
        }

        protected override Task<IPaymentsFlow> LoadAsync(CancellationToken cancellationToken) {
            return Task.FromResult(_paymentsFlow);
        }

        protected override Task UpdateAsync(IPaymentsFlow flow, CancellationToken cancellationToken) {
            _paymentsFlow = flow;
            
            return Task.CompletedTask;
        }
    }
}