using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.NamedParameters;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments {
    public class PaymentsScope : PaymentsScopeBase {
        private readonly IRepository<IPaymentsFlow> _repository;
        private readonly FlowId _flowId;

        public PaymentsScope(IRepository<IPaymentsFlow> repository, FlowId flowId) {
            _repository = repository;
            _flowId = flowId;
        }

        protected override async Task<IPaymentsFlow> LoadAsync(CancellationToken cancellationToken) {
            var flow = await _flowId.RunAsync(_repository.GetAsync, true, cancellationToken);

            return flow;
        }

        protected override async Task UpdateAsync(IPaymentsFlow flow, CancellationToken cancellationToken) {
            await _repository.UpdateAsync(flow, cancellationToken);
        }
    }
}