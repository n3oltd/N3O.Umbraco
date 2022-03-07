using N3O.Umbraco.Extensions;
using N3O.Umbraco.Locks;
using N3O.Umbraco.Payments.Models;
using Stripe;
using System.Linq;
using System.Threading.Tasks;
using StripeCustomerService = Stripe.CustomerService;

namespace N3O.Umbraco.Payments.Stripe.Services {
    public class CustomerService : ICustomerService {
        private readonly ILock _lock;
        private readonly StripeClient _stripeClient;

        public CustomerService(ILock @lock, StripeClient stripeClient) {
            _lock = @lock;
            _stripeClient = stripeClient;
        }

        public async Task<Customer> GetOrCreateCustomerAsync(IBillingInfo billingInfo) {
            var customer = await _lock.LockAsync(nameof(CustomerService), async () => {
                Customer customer;

                var customerService = new StripeCustomerService(_stripeClient);
                var criteria = new CustomerListOptions();
                criteria.Email = billingInfo.Email.Address;

                var customers = await customerService.ListAsync(criteria);

                if (customers.Data.HasAny(x => x.Name.EqualsInvariant(billingInfo.Name.ToFullName()))) {
                    customer = customers.Data.First(x => x.Name.EqualsInvariant(billingInfo.Name.ToFullName()));
                } else {
                    var customerCreateOptions = CreateCustomerOptions(billingInfo);

                    customer = await customerService.CreateAsync(customerCreateOptions);
                }

                return customer;
            });

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