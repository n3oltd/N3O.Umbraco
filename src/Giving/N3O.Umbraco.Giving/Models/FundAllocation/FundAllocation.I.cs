using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models {
    public interface IFundAllocation {
        DonationItem DonationItem { get; }
    }
}