namespace N3O.Umbraco.Cloud.Platforms;

public static class PlatformsConstants {
    public static class Campaigns {
        public const string CompositionAlias = "platformsCampaign";
        public const string ScheduledGiving = "platformsScheduledGivingCampaign";
        public const string Standard = "platformsStandardCampaign";
        public const string Telethon = "platformsTelethonCampaign";
    }

    public static class Designations {
        public const string CompositionAlias = "platformsDesignation";
        public const string SuggestedAmount = "platformsSuggestedAmount";

        public static class Properties {
            public const string Dimension1 = nameof(Dimension1);
            public const string Dimension2 = nameof(Dimension2);
            public const string Dimension3 = nameof(Dimension3);
            public const string Dimension4 = nameof(Dimension4);
        }
    }
    
    public static class FeedbackDesignation {
        public const string Alias = "platformsFeedbackDesignation";

        public static class Properties {
            public const string Scheme = nameof(Scheme);
        }
    }
    
    public static class FundDesignation {
        public const string Alias = "platformsFundDesignation";

        public static class Properties {
            public const string DonationItem = nameof(DonationItem);
        }
    }
    
    public static class SponsorshipDesignation {
        public const string Alias = "platformsSponsorshipDesignation";

        public static class Properties {
            public const string Scheme = nameof(Scheme);
        }
    }

    public static class Elements {
        public const string CompositionAlias = "platformsElement";
        public const string DonateButton = "platformsDonateButtonElement";
        public const string DonationForm = "platformsDonationFormElement";
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
            public const string Theme = "platformsThemeSettings";
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
    }
}