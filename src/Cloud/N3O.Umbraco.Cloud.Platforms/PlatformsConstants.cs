namespace N3O.Umbraco.Cloud.Platforms;

public static class PlatformsConstants {
    public const string BackOfficeApiName = "PlatformsBackOffice";
    
    public static class Campaigns {
        public const string CompositionAlias = "platformsCampaign";
        public const string ScheduledGiving = "platformsScheduledGivingCampaign";
        public const string Standard = "platformsStandardCampaign";
        public const string Telethon = "platformsTelethonCampaign";
    }

    public static class Offerings {
        public const string CompositionAlias = "platformsOffering";
        public const string Feedback = "platformsFeedbackOffering";
        public const string Fund = "platformsFundOffering";
        public const string Sponsorship = "platformsSponsorshipOffering";
        public const string SuggestedAmount = "platformsSuggestedAmount";
    }

    public static class Elements {
        public const string CompositionAlias = "platformsElement";
        public const string DonationButton = "platformsDonationButtonElement";
        public const string DonationForm = "platformsDonationFormElement";
        public const string DonationElement = "platformsDonationElement";
    }

    public static class Platforms {
        public const string Alias = "platforms";
    }

    public static class Settings {
        public const string Alias = "platformsSettings";
        public const string Terminologies = "platformsTerminologies";

        public static class AccountEntry {
            public const string Alias = "platformsAccountEntry";
            public const string Address = "platformsAddressEntry";
            public const string Consent = "platformsConsentEntry";
        }
        
        public static class Build {
            public const string Alias = "platformsBuildSettings";
            public const string Theme = "platformsTheme";
            public const string ThemeSettings = "platformsThemeSettings";
        }

        public static class FundStructure {
            public const string Alias = "platformsFundStructure";
            public const string FundDimension1 = "platformsFundDimension1";
            public const string FundDimension2 = "platformsFundDimension2";
            public const string FundDimension3 = "platformsFundDimension3";
            public const string FundDimension4 = "platformsFundDimension4";
            public const string FundDimensionToggle = "platformsFundDimensionToggle";
        }

        public static class OrganizationInfo {
            public const string Alias = "platformsOrganizationInfo";
        }

        public static class Payments {
            public const string Alias = "platformsPayments";
            public const string Terms = "platformsPaymentTerms";

            public static class BannerAdverts {
                public const string Alias = "platformsBannerAdverts";
                public const string BannerAdvert = "platformsBannerAdvert";
            }
        }

        public static class Tracking {
            public const string Alias = "platformsTracking";
            public const string GoogleAnalytics = "platformsGoogleAnalyticsTracking";
            public const string Meta = "platformsMetaTracking";
            public const string TikTok = "platformsTikTokTracking";
        }
        
        public static class WebhookIds {
            public const string Campaigns = "campaigns";
            public const string Elements = "elements";
            public const string Offerings = "offerings";
            public const string Views = "views";
        }
    }
}