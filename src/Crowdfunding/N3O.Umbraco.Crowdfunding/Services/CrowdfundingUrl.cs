using System;

namespace N3O.Umbraco.Crowdfunding;

public static class CrowdfundingUrl {
    // TODO Talha, need slug helper here so probably not static
    public static string ForCampaign() => throw new NotImplementedException();
    public static string ForFundraiser() => throw new NotImplementedException();
    public static string ForTeam() => throw new NotImplementedException();
    
    public static class Routes {
        public const string Campaign = $"campaigns/([0-9]+)/({Slugs.AllowedCharacters}+)";
        public const string Fundraiser = $"pages/([0-9]+)/({Slugs.AllowedCharacters}+)";
        public const string SignIn = "sign-in";
        public const string Create = $"create/([0-9]+)";

        public static class Slugs {
            public const string AllowedCharacters = @"[a-zA-Z0-9\-]";
            public const string DeniedCharacters = @"[^a-zA-Z0-9\-]";
        }
    }
}