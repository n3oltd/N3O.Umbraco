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
    
    public static class Elements {
        public const string CompositionAlias = "platformsElement";
        public const string DonationButton = "platformsDonationButtonElement";
        public const string DonationForm = "platformsDonationFormElement";
        public const string DonationElement = "platformsDonationElement";
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
        public const string DonationButtons = "donationButtons";
        public const string DonationForms = "donationForms";
        public const string Offerings = "offerings";
        public const string Views = "views";
    }
}