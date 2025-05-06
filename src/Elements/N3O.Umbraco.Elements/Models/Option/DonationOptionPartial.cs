using System;

namespace N3O.Umbraco.Elements.Models;

public class DonationOptionPartial {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string TypeId { get; set; }
    public string DefaultGivingTypeId { get; set; }
    public InitialFundDimensionValueData Dimension1 { get; set; }
    public InitialFundDimensionValueData Dimension2 { get; set; }
    public InitialFundDimensionValueData Dimension3 { get; set; }
    public InitialFundDimensionValueData Dimension4 { get; set; }
    public bool HideQuantity { get; set; }
    public bool HideDonation { get; set; }
    public bool HideRegularGiving { get; set; }
    public bool DefaultOptionInCategory { get; set; }
    public string Synopsis { get; set; }
    public string Description { get; set; }
    public FeedbackDonationOptionData Feedback { get; set; }
    public FundDonationOptionData Fund { get; set; }
    public SponsorshipDonationOptionData Sponsorship { get; set; }
}