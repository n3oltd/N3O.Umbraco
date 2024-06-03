namespace N3O.Umbraco.Crowdfunding;

public static class CrowdfundingConstants {
    public const string ApiName = "Crowdfunding";
    
    public static class Allocations {
        public static class Extensions {
            public const string Key = "Crowdfunding";
        }
    }
    
    public static class CrowdfundingPage {
        public const string Alias = "crowdfundingPage";
        
        public static class Properties {
            public const string PageOwners = "pageOwners";
        }
    }
    
    public static class CrowdfundingPages {
        public const string Alias = "crowdfundingPages";
    }
    
    public static class Tables {
        public static class CrowdfundingContributions {
            public const string Name = "N3O_CrowdfundingContributions";
            public const string PrimaryKey = "PK_N3O_CrowdfundingContributions";
        }
    }
}