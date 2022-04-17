using N3O.Umbraco.Payments.Models;
using Stripe;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Stripe {
    public class Customers : ICustomers {
        private readonly StripeClient _stripeClient;

        public Customers(StripeClient stripeClient) {
            _stripeClient = stripeClient;
        }

        public async Task<Customer> CreateCustomerAsync(IBillingInfo billingInfo) {
            var customerService = new CustomerService(_stripeClient);

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