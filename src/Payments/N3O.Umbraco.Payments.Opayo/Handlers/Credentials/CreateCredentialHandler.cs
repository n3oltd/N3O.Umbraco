using N3O.Umbraco.Context;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class CreateCredentialHandler : PaymentHandler<CreateCredentialCommand, OpayoCredentialReq, OpayoCredential> {
        private readonly IPaymentsFlow _paymentsFlow;

        public CreateCredentialHandler(IActionLinkGenerator actionLinkGenerator,
                                       IPaymentsScope paymentsScope,
                                       IOpayoClient opayoClient,
                                       IPaymentsFlow paymentsFlow,
                                       IRemoteIpAddressAccessor remoteIpAddressAccessor,
                                       IBrowserInfoAccessor userAgentAccessor) :
            base(actionLinkGenerator, paymentsFlow, paymentsScope, opayoClient, remoteIpAddressAccessor, userAgentAccessor) {
            _paymentsFlow = paymentsFlow;
        }

        protected override async Task HandleAsync(CreateCredentialCommand req,
                                                  OpayoCredential credential,
                                                  IBillingInfoAccessor billingInfoAccessor,
                                                  CancellationToken cancellationToken) {
            var payment = _paymentsFlow.GetOrCreatePaymentObject<OpayoPayment>();
            var billingInfo = billingInfoAccessor.GetBillingInfo();
            await ProcessPaymentAsync(payment, req.Model.AdvancePayment, billingInfo, false);
        }
    }
}