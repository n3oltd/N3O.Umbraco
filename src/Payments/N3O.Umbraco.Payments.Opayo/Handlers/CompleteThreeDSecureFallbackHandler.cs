using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Clients;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class CompleteThreeDSecureFallbackHandler :
        CompleteThreeDSecureHandlerBase<CompleteThreeDSecureFallbackCommand, ThreeDSecureFallbackReq> {
        private readonly IOpayoClient _opayoClient;

        public CompleteThreeDSecureFallbackHandler(IPaymentsScope paymentsScope, IOpayoClient opayoClient)
            : base(paymentsScope) {
            _opayoClient = opayoClient;
        }

        protected override async Task<ApiTransactionRes> ProcessThreeDSecureAsync(ThreeDSecureFallbackReq req, OpayoPayment payment, PaymentsParameters parameters) {
            var apiReq = new ApiThreeDSecureFallbackResponse();
            apiReq.PaRes = req.PaRes;
            apiReq.TransactionId = payment.OpayoTransactionId;

            await _opayoClient.CompleteThreeDSecureFallbackResponseAsync(apiReq);

            payment.ThreeDSecureComplete(req.PaRes);

            var transaction = await _opayoClient.GetTransactionByIdAsync(payment.OpayoTransactionId);

            return transaction;
        }
    }
}