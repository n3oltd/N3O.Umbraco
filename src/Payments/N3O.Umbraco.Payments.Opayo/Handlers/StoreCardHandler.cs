using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class StoreCardHandler : PaymentsHandler<StoreCardCommand, StoreCardReq, OpayoCredential> {
        private readonly IChargeService _chargeService;

        public StoreCardHandler(IPaymentsScope paymentsScope, IChargeService chargeService)
            : base(paymentsScope) {
            _chargeService = chargeService;
        }

        protected override async Task HandleAsync(StoreCardCommand req,
                                                  OpayoCredential credential,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            await _chargeService.ChargeAsync((OpayoPayment)credential.AdvancePayment,
                                             req.Model.AdvancePayment,
                                             parameters,
                                             true);
        }
    }
}