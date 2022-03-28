using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Bambora.Client;
using N3O.Umbraco.Payments.Bambora.Commands;
using N3O.Umbraco.Payments.Bambora.Extensions;
using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Bambora.Models.ThreeDSecureChallenge;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Handlers {
    public class CompleteThreeDSecureChallengeHandler :
        PaymentsHandler<CompleteThreeDSecureChallengeCommand, ThreeDSecureChallengeReq, BamboraPayment> {
        private readonly IBamboraPaymentClient _bamboraPaymentClient;

        public CompleteThreeDSecureChallengeHandler(IPaymentsScope paymentsScope, IBamboraPaymentClient bamboraPaymentClient)
            : base(paymentsScope) {
            _bamboraPaymentClient = bamboraPaymentClient;
        }

        protected override async Task HandleAsync(CompleteThreeDSecureChallengeCommand req,
                                                  BamboraPayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            var apiReq = new ThreeDSecureChallenge();
            apiReq.CardResponse = new ThreeDSecureCardResponse();
            apiReq.PaymentMethod = "token";
            apiReq.ThreeDSessionData = req.Model.ThreeDSessionData;
            apiReq.CardResponse.Cres = req.Model.CRes;

            var apiPayment = await _bamboraPaymentClient.CompleteThreeDSecureAsync(apiReq);

            payment.ThreeDSecureComplete(req.Model.CRes);

            if (apiPayment.IsAuthorised()) {
                payment.Paid(apiPayment.Id,
                             apiPayment.MessageId.GetValueOrThrow(),
                             apiPayment.Message);
            } else if (apiPayment.IsDeclined()) {
                payment.Declined(apiPayment.Id, apiPayment.MessageId.GetValueOrThrow(), apiPayment.Message);
            } else {
                throw UnrecognisedValueException.For(apiPayment.Message);
            }
        }
    }
}