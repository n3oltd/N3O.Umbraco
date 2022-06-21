using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public interface IPricing : IPrice {
    IEnumerable<IPricingRule> Rules { get; }
}
