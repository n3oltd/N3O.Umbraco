using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class ChargeCardHandler : PaymentsHandler<ChargeCardCommand, ChargeCardReq, OpayoPayment> {
        private readonly IOpayoHelper _opayoHelper;

        public ChargeCardHandler(IPaymentsScope paymentsScope, IOpayoHelper opayoHelper) : base(paymentsScope) {
            _opayoHelper = opayoHelper;
        }

        protected override async Task HandleAsync(ChargeCardCommand req,
                                                  OpayoPayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            await _opayoHelper.ChargeAsync(payment, req.Model, parameters, false);
        }
    }
}