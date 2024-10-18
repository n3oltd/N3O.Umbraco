namespace N3O.Umbraco.Crowdfunding;

public static class CrowdfundingConstants {
    public const string ApiName = "Crowdfunding";
    
    public static class Allocations {
        public static class Extensions {
            public const string Key = "Crowdfunder";
        }
    }
    
    public static class Block {
        public const string Alias = "crowdfundingBlock";
    }
    
    public static class Campaign {
        public const string Alias = "crowdfundingCampaign";
    }
    
    public static class CampaignGoalOption {
        public static class Feedback {
            public const string Alias = "crowdfundingCampaignFeedbackGoalOption";
        }

        public static class Fund {
            public const string Alias = "crowdfundingCampaignFundGoalOption";
        }
    }

    public static class Crowdfunder {
        public const int NameMaxLength = 200;
        
        public static class Properties {
            public const string BackgroundImage = "backgroundImage";
            public const string Body = "body";
            public const string Currency = "currency";
            public const string Description = "description";
            public const string Goals = "goals";
            public const string HeroImages = "heroImages";
            public const string Name = "displayName";
            public const string OpenGraphImagePath = "openGraphImagePath";
            public const string Status = "status";
            public const string ToggleStatus = "toggleStatus";
        }
    }
    
    public static class EmailTemplates {
        public static class ContributionReceived {
            public const string Alias = "crowdfundingContributionReceivedTemplate";
        }
    }

    public static class Fundraiser {
        public const string Alias = "crowdfundingFundraiser";
        
        public static class Properties {
            public const string AccountReference = "accountReference";
            public const string Campaign = "campaign";
            public const string Owner = "owner";
            public const string Slug = "slug";
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
            public const string Amount = "amount";
            public const string FundDimension1 = "fundDimension1";
            public const string FundDimension2 = "fundDimension2";
            public const string FundDimension3 = "fundDimension3";
            public const string FundDimension4 = "fundDimension4";
            public const string Name = "displayName";
            public const string OptionId = "optionId";
            public const string PriceHandles = "priceHandles";
            public const string Tags = "tags";
        }
    }
    
    public static class HeroImages {
        public const string Alias = "crowdfundingHeroImage";

        public static class Properties {
            public const string Image = "image";
        }
    }
    
    public static class HomePage {
        public const string Alias = "crowdfundingHomePage";
    }
    
    public static class ModuleKeys {
        public const string Block = nameof(Crowdfunding);
    }
    
    public static class Root {
        public const string Alias = "crowdfundingRoot";
    }
    
    public static class Routes {
        public const string CreateFundraiser = "pages/create";
        public const string HomePage = "";
        public const string SearchFundraisers = "pages/search";
        public const string ViewCampaign_2 = "campaigns/{0}/{1}";
        public const string ViewEditFundraiser_2 = "pages/{0}/{1}";
        
        public static class TypedRoutes {
            public const string ViewCampaign = $"campaigns/([0-9]+)/({Slugs.AllowedCharacters}+)";
            public const string ViewEditFundraiser = $"pages/([0-9]+)/({Slugs.AllowedCharacters}+)";
        }

        public static class Slugs {
            public const string AllowedCharacters = @"[a-zA-Z0-9\-]";
            public const string DeniedCharacters = @"[^a-zA-Z0-9\-]";
        }
    }

    public static class Settings {
        public static class OurProfile {
            public const string Alias = "crowdfundingOurProfileSettings";
        }
    
        public static class SignInModal {
            public const string Alias = "crowdfundingSignInModalSettings";
        }
        
        public static class TemplateSettings {
            public const string Alias = "crowdfundingTemplateSettings";
        }
        
        public static class HomePageTemplate {
            public const string Alias = "crowdfundingHomepageTemplate";
        }
    }

    public static class Tables {
        public static class Contributions {
            public const string Name = "N3O_CrowdfundingContributions";
            public const string PrimaryKey = "PK_N3O_CrowdfundingContributions";
        }
        
        public static class Crowdfunders {
            public const string Name = "N3O_CrowdfundingCrowdfunders";
            public const string PrimaryKey = "PK_N3O_CrowdfundingCrowdfunders";
        }
        
        public static class CrowdfunderRevisions {
            public const string Name = "N3O_CrowdfundingCrowdfunderRevisions";
            public const string PrimaryKey = "PK_N3O_CrowdfundingCrowdfunderRevisions";
        }
    }
    
    public static class Tags {
        public const string Alias = "crowdfundingTag";
    }
    
    public static class Webhooks {
        public static class EventTypes {
            public static class Crowdfunder {
                public const string CrowdfunderCreated = "crowdfunder.created";
                public const string CrowdfunderUpdated = "crowdfunder.updated";
            }
            
            public static class Pledges {
                public const string PledgeUpdated = "pledge.updated";
            }
        }

        public static class HookIds {
            public const string Crowdfunder = nameof(Crowdfunder);
            public const string Pledges = nameof(Pledges);
        }
    }
}