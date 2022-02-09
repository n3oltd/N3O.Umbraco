using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Payments.Extensions;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Stripe.Commands;
using N3O.Umbraco.Payments.Stripe.Models;
using Stripe;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Payments.Stripe.Handlers {
    public class CreatePaymentIntentHandler :
        PaymentsHandler<CreatePaymentIntentCommand, PaymentIntentReq, StripePayment> {
        private readonly IContentCache _contentCache;
        private readonly StripeClient _stripeClient;
        
        public CreatePaymentIntentHandler(IPaymentsScope paymentsScope,
                                          IContentCache contentCache,
                                          StripeClient stripeClient)
            : base(paymentsScope) {
            _contentCache = contentCache;
            _stripeClient = stripeClient;
        }

        protected override async Task HandleAsync(CreatePaymentIntentCommand req,
                                                  StripePayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            try {
                var service = new PaymentIntentService(_stripeClient);
            
                var paymentIntentOptions = GetPaymentIntentOptions(parameters, req.Model);
                
                var paymentIntent = await service.CreateAsync(paymentIntentOptions,
                                                              cancellationToken: cancellationToken);
                
                payment.IntentCreated(paymentIntent);
            } catch (StripeException ex) {
                payment.Error(ex);
            }
        }
        
        private PaymentIntentCreateOptions GetPaymentIntentOptions(PaymentsParameters parameters, PaymentIntentReq req) {
            var settings = _contentCache.Single<StripeSettingsContent>();
            
            var options = new PaymentIntentCreateOptions();

            options.Amount = ((Money) req.Value).GetAmountInLowestDenomination();
            options.Confirm = true;
            options.Currency = req.Value.Currency.Code;
            options.Description = settings.GetTransactionDescription(parameters.Reference);
            options.ErrorOnRequiresAction = false;
            options.PaymentMethod = req.PaymentMethodId;
            options.Customer = req.CustomerId;

            options.PaymentMethodTypes = "card".Yield().ToList();
            options.PaymentMethodOptions = new PaymentIntentPaymentMethodOptionsOptions();
            options.PaymentMethodOptions.Card = new PaymentIntentPaymentMethodOptionsCardOptions();
            options.PaymentMethodOptions.Card.Moto = false;

            return options;
        }
    }
}