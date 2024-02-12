namespace N3O.Umbraco.Crowdfunding;

public static class CrowdfundingConstants {
    public const string ApiName = "Crowdfunding";
    
    public static class CrowdfundingAllocation {
        public const string Key = "crowdfunding";
    }
    
    public static class Tables {
        public static class CrowdfundingContributions {
            public const string Name = "N3O_CrowdfundingContributions";
            public const string PrimaryKey = "PK_N3O_CrowdfundingContributions";
        }
    }
    
    public static class CrowdfundingPage {
        public const string Alias = "crowdfundingPage";
        
        public static class Properties {
            public const string AllowedMembers = "allowedMembers";
        }
    }

    
}