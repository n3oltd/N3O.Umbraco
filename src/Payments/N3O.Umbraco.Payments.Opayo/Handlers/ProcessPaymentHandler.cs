using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Context;
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
using Newtonsoft.Json;
using Refit;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class ProcessPaymentHandler : PaymentHandler<ProcessPaymentCommand, OpayoPaymentReq> {
        public ProcessPaymentHandler(IPaymentsScope paymentsScope,
                                     IOpayoClient opayoClient,
                                     IPaymentsFlow paymentsFlow,
                                     IRemoteIpAddressAccessor remoteIpAddressAccessor,
                                     IBrowserInfoAccessor userAgentAccessor) :
            base(paymentsFlow, paymentsScope, opayoClient, remoteIpAddressAccessor, userAgentAccessor) { }

        protected override async Task HandleAsync(ProcessPaymentCommand req,
                                                  OpayoPayment payment,
                                                  IBillingInfoAccessor billingInfoAccessor,
                                                  CancellationToken cancellationToken) {
            var billingInfo = billingInfoAccessor.GetBillingInfo();
            await ProcessPaymentAsync(payment, req.Model, billingInfo, false);
        }
    }
}