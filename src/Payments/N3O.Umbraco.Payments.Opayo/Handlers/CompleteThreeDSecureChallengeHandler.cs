using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Clients;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class CompleteThreeDSecureChallengeHandler :
        CompleteThreeDSecureHandlerBase<CompleteThreeDSecureChallengeCommand, ThreeDSecureChallengeReq> {
        private readonly IOpayoClient _opayoClient;

        public CompleteThreeDSecureChallengeHandler(IPaymentsScope paymentsScope, IOpayoClient opayoClient)
            : base(paymentsScope) {
            _opayoClient = opayoClient;
        }

        protected override async Task<ApiTransactionRes> ProcessThreeDSecureAsync(ThreeDSecureChallengeReq req, OpayoPayment payment, PaymentsParameters parameters) {
            var apiReq = new ApiThreeDSecureChallengeResponse();
            apiReq.CRes = req.CRes;
            apiReq.TransactionId = payment.OpayoTransactionId;

            var transaction = await _opayoClient.CompleteThreeDSecureChallengeResponseAsync(apiReq);

            payment.ThreeDSecureComplete(req.CRes);

            return transaction;
        }
    }
}