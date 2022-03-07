using N3O.Umbraco.Content;
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
    public class CreateSetupIntentHandler :
        PaymentsHandler<CreateSetupIntentCommand, None, StripeCredential> {
        private readonly IContentCache _contentCache;
        private readonly StripeClient _stripeClient;
        private readonly ICustomerService _customerService;

        public CreateSetupIntentHandler(IPaymentsScope paymentsScope,
                                        IContentCache contentCache,
                                        StripeClient stripeClient,
                                        ICustomerService customerService)
            : base(paymentsScope) {
            _contentCache = contentCache;
            _stripeClient = stripeClient;
            _customerService = customerService;
        }

        protected override async Task HandleAsync(CreateSetupIntentCommand req,
                                                  StripeCredential credential,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            try {
                
                var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();
                
                var customer = await _customerService.GetOrCreateCustomerAsync(billingInfo);
                
                var service = new SetupIntentService(_stripeClient);
            
                var setupIntentOptions = GetSetupIntentOptions(parameters, customer);
                
                var setupIntent = await service.CreateAsync(setupIntentOptions, cancellationToken: cancellationToken);
                
                credential.IntentCreated(setupIntent);
            } catch (StripeException ex) {
                credential.Error(ex);
            }
        }
        
        private SetupIntentCreateOptions GetSetupIntentOptions(PaymentsParameters parameters, Customer customer) {
            var settings = _contentCache.Single<StripeSettingsContent>();
            
            var options = new SetupIntentCreateOptions();

            options.PaymentMethodTypes = "card".Yield().ToList();
            options.Customer = customer.Id;
            options.Usage = "off_session";
            options.Description = parameters.GetTransactionDescription(settings);

            options.PaymentMethodOptions = new SetupIntentPaymentMethodOptionsOptions();
            options.PaymentMethodOptions.Card = new SetupIntentPaymentMethodOptionsCardOptions();
            options.PaymentMethodOptions.Card.Moto = false;

            return options;
        }
    }
}