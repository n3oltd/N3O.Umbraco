using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class PlatformsSpecialPages : ISpecialContents {
    public static readonly SpecialContent Campaign = new("campaignPage", "Campaign Page", "campaignPage");
    public static readonly SpecialContent Designation = new("designationPage", "Designation Page", "designationPage");
}