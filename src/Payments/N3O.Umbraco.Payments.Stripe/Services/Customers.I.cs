using N3O.Umbraco.Payments.Models;
using Stripe;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Stripe;

public interface ICustomers {
    Task<Customer> CreateCustomerAsync(IBillingInfo billingInfo);
}
