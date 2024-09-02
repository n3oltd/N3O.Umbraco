using N3O.Umbraco.Constants;
using N3O.Umbraco.Crowdfunding.Content;
using Umbraco.Extensions;

namespace N3O.Umbraco.CrowdFunding.Models;

public partial class ViewEditFundraiserViewModel {
    public class OwnerInfo {
        public string Name { get; set; }
        public string AvatarLink { get; set; }
        
        public static OwnerInfo For(FundraiserContent fundraiser) {
            var owner = new OwnerInfo();
            
            owner.Name = fundraiser.Owner?.Name;
            owner.AvatarLink = fundraiser.Owner?.Value<string>(MemberConstants.Member.Properties.AvatarLink);

            return owner;
        }

    }
}