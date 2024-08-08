using System;

namespace N3O.Umbraco.CrowdFunding.Services;

public class FundraisingPageUrls {
    public const string FundraisingPageRootPath = "/Fundraising";
    public const string FundraisingContentPagePathPattern = "/Pages/([0-9]+)/([a-z0-9-]+)";
    public const string FundraisingSignInPagePath = "/SignIn";

    public static string GetFundraisingPagePath(string path) {
        return path.Substring(path.IndexOf(FundraisingPageRootPath, StringComparison.InvariantCultureIgnoreCase) +
                              FundraisingPageRootPath.Length);
    }
}