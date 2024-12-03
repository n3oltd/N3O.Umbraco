using N3O.Umbraco.Elements.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Models;

public interface IHoldAllowedGivingTypes {
    IEnumerable<GivingType> AllowedGivingTypes { get; }
}
