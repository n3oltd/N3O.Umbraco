using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackSchemeRes : NamedLookupRes {
    public IEnumerable<GivingType> AllowedGivingTypes { get; set; }
    public IEnumerable<FeedbackCustomFieldDefinitionRes> CustomFields { get; set; }
    public IEnumerable<FundDimensionValueRes> Dimension1Options { get; set; }
    public IEnumerable<FundDimensionValueRes> Dimension2Options { get; set; }
    public IEnumerable<FundDimensionValueRes> Dimension3Options { get; set; }
    public IEnumerable<FundDimensionValueRes> Dimension4Options { get; set; }
}
