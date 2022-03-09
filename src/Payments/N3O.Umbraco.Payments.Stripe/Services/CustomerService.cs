using N3O.Umbraco.Locks;
using N3O.Umbraco.Payments.Models;
using Stripe;
using System.Threading.Tasks;
using StripeCustomerService = Stripe.CustomerService;

namespace N3O.Umbraco.Payments.Stripe.Services {
    public class CustomerService : ICustomerService {
        private readonly StripeClient _stripeClient;

        public CustomerService(StripeClient stripeClient) {
            _stripeClient = stripeClient;
        }

        public async Task<Customer> CreateCustomerAsync(IBillingInfo billingInfo) {
            var customerService = new StripeCustomerService(_stripeClient);

            var customerCreateOptions = CreateCustomerOptions(billingInfo);

            var customer = await customerService.CreateAsync(customerCreateOptions);

            return customer;
        }

        private CustomerCreateOptions CreateCustomerOptions(IBillingInfo billingInfo) {
            var options = new CustomerCreateOptions();

            options.Name = billingInfo.Name.ToFullName();
            options.Email = billingInfo.Email.Address;
            options.Phone = billingInfo.Telephone.Number;
            options.Address = billingInfo.Address.ToAddressOptions();

            return options;
        }
    }
}