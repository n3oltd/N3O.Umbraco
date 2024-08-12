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
            public const string Allocations = "allocations";
            public const string Campaign = "campaign";
            public const string Fundraiser = "fundraiser";
            public const string PageOwners = "pageOwners";
            public const string PageSlug = "pageSlug";
            public const string PageStatus = "pageStatus";
            public const string PageTitle = "pageTitle";
        }
    }

    public static class CrowdfundingPageAllocation {
        public const string Alias = "crowdfundingPageAllocation";

        public static class Properties {
            public const string Title = "title";
            public const string Type = "type";
            public const string Amount = "amount";
            public const string FundDimension1 = "fundDimension1";
            public const string FundDimension2 = "fundDimension2";
            public const string FundDimension3 = "fundDimension3";
            public const string FundDimension4 = "fundDimension4";
            public const string PriceHandles = "priceHandles";
            
            public static class PriceHandle {
                public const string Alias = "priceHandle";

                public static class Properties {
                    public const string Amount = "amount";
                    public const string Description = "description";
                }
            }
        }
    }

    public static class CrowdfundingPageFundAllocation {
        public const string Alias = "crowdfundingPageFundAllocation";

        public static class Properties {
            public const string DonationItem = "donationItem";
        }
    }
    
    public static class CrowdfundingPageSponsorshipAllocation {
        public const string Alias = "crowdfundingPageSponsorshipAllocation";

        public static class Properties {
            public const string Scheme = "scheme";
        }
    }
    
    public static class CrowdfundingPageFeedbackAllocation {
        public const string Alias = "crowdfundingPageFeedbackAllocation";

        public static class Properties {
            public const string Scheme = "scheme";
            public const string CustomFields = "customFields";
        }
    }
    
    public static class CrowdfundingPages {
        public const string Alias = "crowdfundingPages";
    }

    public static class FundraisingPage {
        public const string Alias = "fundraisingPage";
    }

    public static class Tables {
        public static class CrowdfundingContributions {
            public const string Name = "N3O_CrowdfundingContributions";
            public const string PrimaryKey = "PK_N3O_CrowdfundingContributions";
        }
    }
}