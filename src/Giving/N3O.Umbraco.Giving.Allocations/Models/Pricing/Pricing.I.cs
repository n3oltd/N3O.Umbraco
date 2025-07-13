using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IPricing {
    IPrice Price { get; }
    IEnumerable<IPricingRule> Rules { get; }
}
