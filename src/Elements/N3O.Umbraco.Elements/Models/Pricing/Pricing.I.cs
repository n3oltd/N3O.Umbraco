using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Models;

public interface IPricing : IPrice {
    IEnumerable<IPricingRule> Rules { get; }
}
