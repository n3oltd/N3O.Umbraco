using GoCardless;
using GoCardless.Services;
using N3O.Umbraco.Payments.GoCardless.Commands;
using N3O.Umbraco.Payments.GoCardless.Models;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.GoCardless.Handlers {
    public class CompleteRedirectFlowHandler :
        PaymentsHandler<CompleteRedirectFlowCommand, None, GoCardlessCredential> {
        private readonly GoCardlessClient _goCardlessClient;

        public CompleteRedirectFlowHandler(GoCardlessClient goCardlessClient, IPaymentsScope paymentsScope)
            : base(paymentsScope) {
            _goCardlessClient = goCardlessClient;
        }

        protected override async Task HandleAsync(CompleteRedirectFlowCommand req,
                                                  GoCardlessCredential credential,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            if (credential.Status == PaymentObjectStatuses.InProgress) {
                var completeRequest = new RedirectFlowCompleteRequest();
                completeRequest.SessionToken = credential.GoCardlessSessionToken;

                var response = await _goCardlessClient.RedirectFlows.CompleteAsync(credential.GoCardlessRedirectFlowId,
                                                                                   completeRequest);

                credential.CompleteRedirectFlow(response.RedirectFlow.Links.Customer,
                                                response.RedirectFlow.Links.Mandate);
            }
        }
    }
}