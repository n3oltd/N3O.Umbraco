using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Stripe.Commands;
using N3O.Umbraco.Payments.Stripe.Models;
using N3O.Umbraco.Payments.Stripe.Services;
using Stripe;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Payments.Stripe.Handlers {
    public class CreatePaymentIntentHandler :
        PaymentsHandler<CreatePaymentIntentCommand, PaymentIntentReq, StripePayment> {
        private readonly IPaymentsScope _paymentsScope;
        private readonly IContentCache _contentCache;
        private readonly StripeClient _stripeClient;
        private readonly ICustomers _customers;

        public CreatePaymentIntentHandler(IPaymentsScope paymentsScope,
                                          IContentCache contentCache,
                                          StripeClient stripeClient,
                                          ICustomers customers)
            : base(paymentsScope) {
            _paymentsScope = paymentsScope;
            _contentCache = contentCache;
            _stripeClient = stripeClient;
            _customers = customers;
        }

        protected override async Task HandleAsync(CreatePaymentIntentCommand req,
                                                  StripePayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            try {
                var settings = _contentCache.Single<StripeSettingsContent>();
                var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();
                var customer = await _customers.CreateCustomerAsync(billingInfo);
                var service = new PaymentIntentService(_stripeClient);
                var paymentIntentOptions = GetPaymentIntentOptions(parameters, req.Model, customer);
                
                var options = new RequestOptions();
                options.IdempotencyKey = parameters.GetTransactionId(settings, req.Model.PaymentMethodId);

                var paymentIntent = await service.CreateAsync(paymentIntentOptions, options, cancellationToken);

                payment.IntentCreated(paymentIntent);
            } catch (StripeException ex) {
                payment.Error(ex);
            }
        }
        
        private PaymentIntentCreateOptions GetPaymentIntentOptions(PaymentsParameters parameters, PaymentIntentReq req, Customer customer) {
            var settings = _contentCache.Single<StripeSettingsContent>();
            var options = new PaymentIntentCreateOptions();

            options.Amount = ((Money) req.Value).GetAmountInLowestDenomination();
            options.Currency = req.Value.Currency.Code;
            options.Description = parameters.GetTransactionDescription(settings);
            options.ErrorOnRequiresAction = false;
            options.Customer = customer.Id;
            options.PaymentMethod = req.PaymentMethodId;
            options.Confirm = true;
            options.ConfirmationMethod = "manual";
            
            options.PaymentMethodTypes = "card".Yield().ToList();
            options.PaymentMethodOptions = new PaymentIntentPaymentMethodOptionsOptions();
            options.PaymentMethodOptions.Card = new PaymentIntentPaymentMethodOptionsCardOptions();
            options.PaymentMethodOptions.Card.Moto = false;

            return options;
        }
    }
}