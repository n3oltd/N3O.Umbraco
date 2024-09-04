using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using NodaTime;

namespace N3O.Umbraco.CrowdFunding.Models;

public partial class FundraiserOrCampaignViewModel<TContent> {
    public class Contribution {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string AvatarLink { get; set; }
        public LocalDate DonatedAt { get; set; }
        public bool IsAnonymous { get; set; }
        public Money Amount { get; set; }

        public static Contribution For(ICrowdfundingHelper crowdfundingHelper, OnlineContribution src) {
            var dest = new Contribution();
            
            dest.IsAnonymous = src.Anonymous;
            dest.Comment = src.Comment;
            dest.DonatedAt = src.Timestamp.ToLocalDate();
            dest.Amount = crowdfundingHelper.GetQuoteMoney(src.BaseAmount);
            dest.AvatarLink = (src.Name ?? "Anonymous").GetGravatarUrl();

            if (!dest.IsAnonymous) {
                dest.Name = src.Name;
            }

            return dest;
        }
    }
}