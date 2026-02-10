namespace N3O.Umbraco.Cloud.Platforms;

public static class PlatformsConstants {
    public const string BackOfficeApiName = "PlatformsBackOffice";
    public const string DevToolsApiName = "PlatformsDevTools";
    
    public static class Campaigns {
        public const string CompositionAlias = "platformsCampaign";
        public const string ScheduledGiving = "platformsScheduledGivingCampaign";
        public const string Standard = "platformsStandardCampaign";
        public const string Telethon = "platformsTelethonCampaign";
    }
    
    public static class CrowdfundingCampaign {
        public const string CompositionAlias = "platformsCrowdfundingCampaign";
    }
    
    public static class Elements {
        public const string CompositionAlias = "platformsElement";
        public const string CreateCrowdfunderButton = "platformsCreateCrowdfunderButton";
        public const string DonationButton = "platformsDonationButtonElement";
        public const string DonationElement = "platformsDonationElement";
        public const string DonationForm = "platformsDonationFormElement";
        public const string DonationPopup = "platformsDonationPopupElement";
    }

    public static class Feeds {
        public const string Alias = "feeds";

        public static class Feed {
            public const string Alias = "feed";
        }

        public static class Item {
            public const string Alias = "feedsItem";
        }
        
        public static class Folders {
            public const string ApprovedFolder = "feedsApprovedFolder";
            public const string RejectedFolder = "feedsRejectedFolder";
            public const string ArchivedFolder = "feedsArchivedFolder";
        }
    }

    public static class Offerings {
        public const string CompositionAlias = "platformsOffering";
        public const string Feedback = "platformsFeedbackOffering";
        public const string Fund = "platformsFundOffering";
        public const string Sponsorship = "platformsSponsorshipOffering";
        public const string SuggestedAmount = "platformsSuggestedAmount";
    }

    public static class Platforms {
        public const string Alias = "platforms";
    }

    public static class WebhookIds {
        public const string Campaigns = "campaigns";
        public const string ContentCollection = "contentCollection";
        public const string ContentLibrary = "contentLibrary";
        public const string CrowdfundingCampaigns = "crowdfundingCampaigns";
        public const string DonationButtons = "donationButtons";
        public const string DonationForms = "donationForms";
        public const string DonationPopups = "donationPopups";
        public const string ManagedContent = "managedContent";
        public const string Offerings = "offerings";
        public const string ZakatSettings = "zakatSettings";
    }
    
    public static class Zakat {
        public static class Settings {
            public static class Calculator {
                public const string Alias = "zakatCalculatorSettings";

                public static class Field {
                    public const string Alias = "zakatCalculatorFieldSettings";
                }

                public static class Section {
                    public const string Alias = "zakatCalculatorSectionSettings";
                }
            }
        }
    }
}