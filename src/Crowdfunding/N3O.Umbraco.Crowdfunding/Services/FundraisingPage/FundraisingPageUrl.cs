using System;

namespace N3O.Umbraco.CrowdFunding.Services;

public class FundraisingPageUrl {
    public const string FundraisingPageRootPathPattern = "/Fundraising";
    public const string FundraisingPagePathPattern = "/Pages/([0-9]+)/([a-z0-9-]+)";
    public const string FundraisingSignInPagePath = "/SignIn";
    public const string FundraisingCreatePagePath = "/Pages/Create";

    public static string GetFundraisingPagePath(string path) {
        return path.Substring(path.IndexOf(FundraisingPageRootPathPattern, StringComparison.InvariantCultureIgnoreCase) +
                              FundraisingPageRootPathPattern.Length);
    }
}