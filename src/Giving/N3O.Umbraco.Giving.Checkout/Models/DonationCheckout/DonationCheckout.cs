using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Payments.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class DonationCheckout : Value {
    [JsonConstructor]
    public DonationCheckout(IEnumerable<Allocation> allocations, Payment payment, Money total) {
        Allocations = allocations.OrEmpty().ToList();
        Payment = payment;
        Total = total;
    }

    public DonationCheckout(IEnumerable<Allocation> allocations, Currency currency)
        : this(allocations.OrEmpty(),
               null,
               allocations.HasAny() ? allocations.Select(x => x.Value).Sum() : currency.Zero()) { }

    public IEnumerable<Allocation> Allocations { get; }
    public Payment Payment { get; }
    public Money Total { get; }

    public bool IsComplete => IsRequired && Payment?.IsPaid == true;
    public bool IsRequired => Allocations.HasAny();
    
    public IEnumerable<CheckoutPledge> Pledges => Allocations.Where(x => x.PledgeUrl.HasValue())
                                                             .Select(x => new CheckoutPledge(x.PledgeUrl))
                                                             .ToList();

    public DonationCheckout UpdatePayment(Payment payment) {
        return new DonationCheckout(Allocations, payment, Total);
    }
}
