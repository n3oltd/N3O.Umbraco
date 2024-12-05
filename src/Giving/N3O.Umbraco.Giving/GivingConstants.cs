namespace N3O.Umbraco.Giving;

public static class GivingConstants {
    public const string ApiName = "Giving";

    public static class Webhooks {
        public static class EventTypes {
            public const string Published = "donationItem.published";
            public const string Unpublished = "donationItem.unpublished";
        }
        
        public static class HookIds {
            public const string DonationItem = nameof(DonationItem);
        }

        public static class Headers {
            public static readonly string PreviousName = "N3O-Donation-Item-Previous-Name";
        }
    }
}
