using N3O.Umbraco.Context;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class ProcessPaymentHandler : PaymentHandler<ProcessPaymentCommand, OpayoPaymentReq, OpayoPayment> {
        public ProcessPaymentHandler(IActionLinkGenerator actionLinkGenerator,
                                     IPaymentsScope paymentsScope,
                                     IOpayoClient opayoClient,
                                     IPaymentsFlow paymentsFlow,
                                     IRemoteIpAddressAccessor remoteIpAddressAccessor,
                                     IBrowserInfoAccessor userAgentAccessor) :
            base(actionLinkGenerator, paymentsFlow, paymentsScope, opayoClient, remoteIpAddressAccessor, userAgentAccessor) { }

        protected override async Task HandleAsync(ProcessPaymentCommand req,
                                                  OpayoPayment payment,
                                                  IBillingInfoAccessor billingInfoAccessor,
                                                  CancellationToken cancellationToken) {
            var billingInfo = billingInfoAccessor.GetBillingInfo();
            await ProcessPaymentAsync(payment, req.Model, billingInfo, false);
        }
    }
}