namespace N3O.Umbraco.Elements.Models;

public class PricingRuleData {
    public FundDimensionValuesData FundDimensions { get; set; }
    public decimal Amount { get; set; }
    public bool Locked { get; set; }
}
