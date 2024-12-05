using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Queries;

public class GetDonationFormByIdQuery : Request<None, DonationFormRes> {
    public GetDonationFormByIdQuery(DonationFormId donationFormId) {
        DonationFormId = donationFormId;
    }
    
    public DonationFormId DonationFormId { get; }
}
