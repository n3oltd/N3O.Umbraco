using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingPageUrlConstants {
    public const string CrowdfundingContentPagePathPattern = "/Pages/([0-9]+)/([a-z0-9-]+)";
    public const string CrowdfundingSignInPagePath = "/SignIn";
    
    public static string CrowdfundingPageRootPath;

    public static string GetCrowdfundingPagePath(string path) {
        return path.Substring(path.IndexOf(CrowdfundingPageRootPath, StringComparison.InvariantCultureIgnoreCase) +
                              CrowdfundingPageRootPath.Length);
    }
    
    public static void SetRootPath(string path) {
        if (path.EqualsInvariant(CrowdfundingPageRootPath)) {
            return;
        }

        CrowdfundingPageRootPath = path;
    }
}