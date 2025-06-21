using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.Alias)]
public class SettingsContent : UmbracoContent<SettingsContent> {
    public BuildSettingsContent Build => Content().GetSingleChildOfTypeAs<BuildSettingsContent>();
    public DataEntryContent DataEntry => Content().GetSingleChildOfTypeAs<DataEntryContent>();
    public FundStructureContent FundStructure => Content().GetSingleChildOfTypeAs<FundStructureContent>();
    public OrganizationInfoContent OrganizationInfo => Content().GetSingleChildOfTypeAs<OrganizationInfoContent>();
    public PaymentsSettingsContent PaymentsSettings => Content().GetSingleChildOfTypeAs<PaymentsSettingsContent>();
    public TerminologiesContent Terminologies => Content().GetSingleChildOfTypeAs<TerminologiesContent>();
    public TrackingContent Tracking => Content().GetSingleChildOfTypeAs<TrackingContent>();
}