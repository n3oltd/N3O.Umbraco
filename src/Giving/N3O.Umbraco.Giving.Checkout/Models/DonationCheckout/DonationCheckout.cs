using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Payments.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class DonationCheckout : Value {
        [JsonConstructor]
        public DonationCheckout(IEnumerable<Allocation> allocations, Payment payment, Money total) {
            Allocations = allocations.OrEmpty().ToList();
            Payment = payment;
            Total = total;
        }

        public DonationCheckout(IEnumerable<Allocation> allocations)
            : this(allocations, null, allocations.Select(x => x.Value).Sum()) { }

        public IEnumerable<Allocation> Allocations { get; }
        public Payment Payment { get; }
        public Money Total { get; }
    }
}