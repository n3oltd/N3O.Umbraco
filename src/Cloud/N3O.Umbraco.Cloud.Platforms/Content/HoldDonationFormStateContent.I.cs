using N3O.Umbraco.Cloud.Platforms.Clients;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public interface IHoldDonationFormStateContent {
    DonationFormStateContent FormState { get; }

    void PopulateContributionInfo(ICdnClient cdnClient, PlatformsContributionInfoReq platformsContribution);
    void PopulateOptions(DonationFormOptionsReq options);
}