using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Models;

public class DonationOptionRes {
    public int Id { get; set; }
    public string Name { get; set; }
    public string CampaignName { get; set; }
    public AllocationType Type { get; set; }
    public GivingType DefaultGivingType { get; set; }
    public InitialFundDimensionValueRes Dimension1 { get; set; }
    public InitialFundDimensionValueRes Dimension2 { get; set; }
    public InitialFundDimensionValueRes Dimension3 { get; set; }
    public InitialFundDimensionValueRes Dimension4 { get; set; }
    public bool HideQuantity { get; set; }
    public bool HideDonation { get; set; }
    public bool HideRegularGiving { get; set; }
    public FundDonationOptionRes Fund { get; set; }
    public SponsorshipDonationOptionRes Sponsorship { get; set; }
    public FeedbackDonationOptionRes Feedback { get; set; }
}
