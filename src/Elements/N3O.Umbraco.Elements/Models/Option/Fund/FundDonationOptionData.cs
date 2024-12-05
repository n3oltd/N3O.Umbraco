using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Models;

public class FundDonationOptionData {
    public string DonationItemId { get; set; }
    public string DonationItemName { get; set; }
    public IEnumerable<string> AllowedGivingTypeIds { get; set; }
    public IEnumerable<FundDimensionValueData> Dimension1Options { get; set; }
    public IEnumerable<FundDimensionValueData> Dimension2Options { get; set; }
    public IEnumerable<FundDimensionValueData> Dimension3Options { get; set; }
    public IEnumerable<FundDimensionValueData> Dimension4Options { get; set; }
    public PricingData Pricing { get; set; }
    public IEnumerable<PriceHandleData> DonationPriceHandles { get; set; }
    public IEnumerable<PriceHandleData> RegularGivingPriceHandles { get; set; }
}
