using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class StoreCardHandler : PaymentsHandler<StoreCardCommand, StoreCardReq, OpayoCredential> {
        private readonly IOpayoHelper _opayoHelper;

        public StoreCardHandler(IPaymentsScope paymentsScope, IOpayoHelper opayoHelper) : base(paymentsScope) {
            _opayoHelper = opayoHelper;
        }

        protected override async Task HandleAsync(StoreCardCommand req,
                                                  OpayoCredential credential,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            await DoAsync<OpayoPayment>(async payment => {
                await _opayoHelper.ChargeAsync(payment, req.Model.AdvancePayment, parameters, true);
            }, cancellationToken);
        }
    }
}