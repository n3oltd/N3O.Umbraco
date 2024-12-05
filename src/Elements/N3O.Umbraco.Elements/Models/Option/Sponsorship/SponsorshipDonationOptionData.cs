using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Models;

public class SponsorshipDonationOptionData {
    public string SchemeId { get; set; }
    public string SchemeName { get; set; }
    public IEnumerable<string> AllowedGivingTypeIds { get; set; }
    public IEnumerable<SponsorshipDurationData> AllowedDurations { get; set; }
    public IEnumerable<FundDimensionValueData> Dimension1Options { get; set; }
    public IEnumerable<FundDimensionValueData> Dimension2Options { get; set; }
    public IEnumerable<FundDimensionValueData> Dimension3Options { get; set; }
    public IEnumerable<FundDimensionValueData> Dimension4Options { get; set; }
    public IEnumerable<SponsorshipComponentData> Components { get; set; }
}
