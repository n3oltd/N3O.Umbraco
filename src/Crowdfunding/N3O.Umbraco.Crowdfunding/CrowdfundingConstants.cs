namespace N3O.Umbraco.Crowdfunding;

public static class CrowdfundingConstants {
    public const string ApiName = "Crowdfunding";
    
    public static class Allocations {
        public static class Extensions {
            public const string Key = "Crowdfunding";
        }
    }
    
    public static class Campaign {
        public const string Alias = "crowdfundingCampaign";
    }
    
    public static class CrowdfundingHomePage {
        public const string Alias = "crowdfundingHomePage";
    }

    public static class Fundraiser {
        public const string Alias = "crowdfundingFundraiser";
        
        public static class Properties {
            public const string AccountReference = "accountReference";
            public const string Currency = "currency";
            public const string Goals = "goals";
            public const string Body = "body";
            public const string Campaign = "campaign";
            public const string Description = "description";
            public const string DisplayName = "displayName";
            public const string EndDate = "endDate";
            public const string HeroImages = "heroImages";
            public const string Owner = "owner";
            public const string Slug = "slug";
            public const string Status = "status";
            public const string Title = "title";
        }
    }
    
    public static class Fundraisers {
        public const string Alias = "crowdfundingFundraisers";
    }

    public static class Goal {
        public static class Feedback {
            public const string Alias = "crowdfundingFeedbackGoal";
            
            public static class Properties {
                public const string CustomFields = "customFields";
                public const string Scheme = "scheme";
            }
        }

        public static class Fund {
            public const string Alias = "crowdfundingFundGoal";


            public static class Properties {
                public const string DonationItem = "donationItem";
            }
        }
        
        public static class Properties {
            public const string Title = "title";
            public const string Amount = "amount";
            public const string FundDimension1 = "fundDimension1";
            public const string FundDimension2 = "fundDimension2";
            public const string FundDimension3 = "fundDimension3";
            public const string FundDimension4 = "fundDimension4";
            public const string PriceHandles = "priceHandles";
            public const string Tags = "tags";
        }
    }
    
    public static class Tables {
        public static class OfflineContributions {
            public const string Name = "N3O_CrowdfundingOfflineContributions";
            public const string PrimaryKey = "PK_N3O_CrowdfundingOfflineContributions";
        }
        
        public static class OnlineContributions {
            public const string Name = "N3O_OnlineContributions";
            public const string PrimaryKey = "PK_N3O_OnlineContributions";
        }
    }
    
    public static class Team {
        public const string Alias = "crowdfundingTeam";
    }
    
    public static class Webhooks {
        public static class EventTypes {
            public static class Pledges {
                public const string PledgeUpdated = "pledge.updated";
            }
        }

        public static class HookIds {
            public const string Pledges = nameof(Pledges);
        }
    }
}