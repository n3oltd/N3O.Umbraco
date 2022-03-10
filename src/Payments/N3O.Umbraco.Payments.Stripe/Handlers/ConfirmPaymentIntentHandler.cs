using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Stripe.Commands;
using N3O.Umbraco.Payments.Stripe.Models;
using Stripe;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Stripe.Handlers {
    public class ConfirmPaymentIntentHandler : PaymentsHandler<ConfirmPaymentIntentCommand, None, StripePayment> {
        private readonly StripeClient _stripeClient;
        
        public ConfirmPaymentIntentHandler(IPaymentsScope paymentsScope, StripeClient stripeClient)
            : base(paymentsScope) {
            _stripeClient = stripeClient;
        }

        protected override async Task HandleAsync(ConfirmPaymentIntentCommand req,
                                                  StripePayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            try {
                var service = new PaymentIntentService(_stripeClient);
            
                var paymentIntent = await service.ConfirmAsync(payment.StripePaymentIntentId,
                                                               cancellationToken: cancellationToken);
                
                payment.IntentConfirmed(paymentIntent);
            } catch (StripeException ex) {
                payment.Error(ex);
            }
        }
    }
}