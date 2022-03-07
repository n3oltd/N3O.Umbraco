using N3O.Umbraco.Payments.Models;
using Stripe;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Stripe.Services {
    public interface ICustomerService {
        Task<Customer> GetOrCreateCustomerAsync(IBillingInfo billingInfo);
    }
}