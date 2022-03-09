using FluentValidation;
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
            
                //TODO add the stripePaymentMethod as idempotency key to this request \|/
                // var options = new RequestOptions();
                // options.IdempotencyKey = parameters.GetTransactionDescription();
                
                var paymentIntent = await service.GetAsync(payment.StripePaymentIntentId,
                                                           cancellationToken: cancellationToken);
                
                if (paymentIntent.Status == "requires_payment_method") {
                    throw new ValidationException("Payment method cannot be attached to payment intent in the current state.");
                }
                
                payment.Confirm(paymentIntent);
            } catch (StripeException ex) {
                payment.Error(ex);
            }
        }
    }
}