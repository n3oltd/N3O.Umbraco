using N3O.Umbraco.Financial;
using System;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public class FundraisingContentPageDonations {
    public string Name { get; set; }
    public string Comment { get; set; }
    public string AvatarLink { get; set; }
    public DateTime DonatedAt { get; set; }
    public bool IsAnonymous { get; set; }
    public Money Amount { get; set; }
}