namespace N3O.Umbraco.Crowdfunding;

public static class CrowdfundingConstants {
    public const string ApiName = "Crowdfunding";
    
    public static class Allocations {
        public static class Extensions {
            public static readonly string Key = "Crowdfunder";
        }
    }
    
    public static class Block {
        public static readonly string Alias = "crowdfundingBlock";
    }
    
    public static class Campaign {
        public const string Alias = "crowdfundingCampaign";
    }
    
    public static class CampaignGoalOption {
        public static class Feedback {
            public static readonly string Alias = "crowdfundingCampaignFeedbackGoalOption";
        }

        public static class Fund {
            public static readonly string Alias = "crowdfundingCampaignFundGoalOption";
        }
    }

    public static class Crowdfunder {
        public const int NameMaxLength = 200;
        
        public static class Properties {
            public static readonly string BackgroundImage = "backgroundImage";
            public static readonly string Body = "body";
            public static readonly string Currency = "currency";
            public static readonly string Description = "description";
            public static readonly string Goals = "goals";
            public static readonly string HeroImages = "heroImages";
            public const string Name = "displayName";
            public static readonly string OpenGraphImagePath = "openGraphImagePath";
            public static readonly string Status = "status";
            public static readonly string ToggleStatus = "toggleStatus";
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
            public static readonly string AccountReference = "accountReference";
            public static readonly string Campaign = "campaign";
            public static readonly string Owner = "owner";
            public static readonly string Slug = "slug";
        }
    }
    
    public static class Fundraisers {
        public static readonly string Alias = "crowdfundingFundraisers";
    }

    public static class Goal {
        public static class Feedback {
            public static readonly string Alias = "crowdfundingFeedbackGoal";
            
            public static class Properties {
                public static readonly string CustomFields = "customFields";
                public static readonly string Scheme = "scheme";
            }
        }

        public static class Fund {
            public static readonly string Alias = "crowdfundingFundGoal";

            public static class Properties {
                public static readonly string DonationItem = "donationItem";
            }
        }
        
        public static class Properties {
            public static readonly string Amount = "amount";
            public static readonly string FundDimension1 = "fundDimension1";
            public static readonly string FundDimension2 = "fundDimension2";
            public static readonly string FundDimension3 = "fundDimension3";
            public static readonly string FundDimension4 = "fundDimension4";
            public const string Name = "displayName";
            public static readonly string OptionId = "optionId";
            public static readonly string PriceHandles = "priceHandles";
            public static readonly string Tags = "tags";
        }
    }
    
    public static class HeroImages {
        public static readonly string Alias = "crowdfundingHeroImage";

        public static class Properties {
            public static readonly string Image = "image";
        }
    }
    
    public static class HomePage {
        public const string Alias = "crowdfundingHomePage";
    }
    
    public static class ModuleKeys {
        public static readonly string Block = nameof(Crowdfunding);
    }
    
    public static class Root {
        public static readonly string Alias = "crowdfundingRoot";
    }
    
    public static class Routes {
        public static readonly string CreateFundraiser = "pages/create";
        public static readonly string HomePage = "";
        public static readonly string SearchFundraisers = "pages/search";
        public static readonly string ViewCampaign_2 = "campaigns/{0}/{1}";
        public static readonly string ViewEditFundraiser_2 = "pages/{0}/{1}";
        
        public static class TypedRoutes {
            public static readonly string ViewCampaign = $"campaigns/([0-9]+)/({Slugs.AllowedCharacters}+)";
            public static readonly string ViewEditFundraiser = $"pages/([0-9]+)/({Slugs.AllowedCharacters}+)";
        }

        public static class Slugs {
            public static readonly string AllowedCharacters = @"[a-zA-Z0-9\-]";
            public static readonly string DeniedCharacters = @"[^a-zA-Z0-9\-]";
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