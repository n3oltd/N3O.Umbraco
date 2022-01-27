using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Extensions;
using N3O.Umbraco.Payments.Opayo.Models;
using N3O.Umbraco.Payments.Testing;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class ThreeDSecureChallengeHandler :
        PaymentsHandler<ThreeDSecureChallengeCommand, ThreeDSecureChallengeReq, OpayoPayment> {
        private readonly IOpayoClient _opayoClient;
        private readonly IPaymentsFlow _paymentsFlow;

        public ThreeDSecureChallengeHandler(IPaymentsScope paymentsScope,
                                            IOpayoClient opayoClient,
                                            IPaymentsFlow paymentsFlow) : base(paymentsScope) {
            _opayoClient = opayoClient;
            _paymentsFlow = paymentsFlow;
        }

        protected override async Task HandleAsync(ThreeDSecureChallengeCommand req,
                                                  OpayoPayment payment,
                                                  IBillingInfoAccessor billingInfoAccessor,
                                                  CancellationToken cancellationToken) {
            var apiReq = new ApiThreeDSecureChallenge();
            apiReq.CRes = req.Model.CRes;
            apiReq.TransactionId = payment.TransactionId;

            var transaction = await _opayoClient.ThreeDSecureChallengeAsync(apiReq);

            if (transaction.IsAuthorised()) {
                payment.Paid(transaction.TransactionId, transaction.BankAuthorisationCode, transaction.RetrievalReference.GetValueOrThrow());
            } else if (transaction.IsDeclined()) {
                payment.Declined(transaction.StatusDetail);
            }

            payment.ThreeDSecureProcessCompleted();
        }
    }
}