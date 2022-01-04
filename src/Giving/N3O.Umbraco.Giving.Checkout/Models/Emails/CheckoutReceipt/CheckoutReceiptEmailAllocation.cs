using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutReceiptEmailAllocation {
        public CheckoutReceiptEmailAllocation(string summary,
                                              string donationType,
                                              string fundDimension1,
                                              string fundDimension2,
                                              string fundDimension3,
                                              string fundDimension4,
                                              Money value,
                                              string valueText) {
            Summary = summary;
            DonationType = donationType;
            FundDimension1 = fundDimension1;
            FundDimension2 = fundDimension2;
            FundDimension3 = fundDimension3;
            FundDimension4 = fundDimension4;
            Value = value;
            ValueText = valueText;
        }

        public string Summary { get; }
        public string DonationType { get; }
        public string FundDimension1 { get; }
        public string FundDimension2 { get; }
        public string FundDimension3 { get; }
        public string FundDimension4 { get; }
        public Money Value { get; }
        public string ValueText { get; }
    }
}