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
        private readonly ICustomerService _customerService;

        public CreatePaymentIntentHandler(IPaymentsScope paymentsScope,
                                          IContentCache contentCache,
                                          StripeClient stripeClient,
                                          ICustomerService customerService)
            : base(paymentsScope) {
            _paymentsScope = paymentsScope;
            _contentCache = contentCache;
            _stripeClient = stripeClient;
            _customerService = customerService;
        }

        protected override async Task HandleAsync(CreatePaymentIntentCommand req,
                                                  StripePayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            try {
                var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();
                
                var customer = await _customerService.GetOrCreateCustomerAsync(billingInfo);

                var service = new PaymentIntentService(_stripeClient);
            
                var paymentIntentOptions = CreatePaymentIntentOptions(parameters, req.Model, customer);
                
                var paymentIntent = await service.CreateAsync(paymentIntentOptions,
                                                              cancellationToken: cancellationToken);
                
                payment.IntentCreated(paymentIntent);
            } catch (StripeException ex) {
                payment.Error(ex);
            }
        }
        
        private PaymentIntentCreateOptions CreatePaymentIntentOptions(PaymentsParameters parameters, PaymentIntentReq req, Customer customer) {
            var settings = _contentCache.Single<StripeSettingsContent>();
            
            var options = new PaymentIntentCreateOptions();

            options.Amount = ((Money) req.Value).GetAmountInLowestDenomination();
            options.Currency = req.Value.Currency.Code;
            options.Description = parameters.GetTransactionDescription(settings);
            options.ErrorOnRequiresAction = false;
            options.Customer = customer.Id;

            options.PaymentMethodTypes = "card".Yield().ToList();
            options.PaymentMethodOptions = new PaymentIntentPaymentMethodOptionsOptions();
            options.PaymentMethodOptions.Card = new PaymentIntentPaymentMethodOptionsCardOptions();
            options.PaymentMethodOptions.Card.Moto = false;

            return options;
        }
        
        private CustomerCreateOptions CreateCustomerOptions(PaymentsParameters parameters) {
            var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();
            
            var options = new CustomerCreateOptions();

            options.Name = $"{billingInfo.Name.FirstName} {billingInfo.Name.LastName}";
            options.Email = billingInfo.Email.Address;
            options.Phone = billingInfo.Telephone.Number;
            options.Address = billingInfo.Address.ToAddressOptions();

            return options;
        }
    }
}