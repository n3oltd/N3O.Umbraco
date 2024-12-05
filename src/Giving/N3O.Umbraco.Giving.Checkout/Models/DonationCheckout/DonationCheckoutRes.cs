using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Payments.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Checkout.Models;

public class DonationCheckoutRes {
    public IEnumerable<AllocationRes> Allocations { get; set; }
    public PaymentRes Payment { get; set; }
    public MoneyRes Total { get; set; }

    public bool IsComplete { get; set; }
    public bool IsRequired { get; set; }
}
