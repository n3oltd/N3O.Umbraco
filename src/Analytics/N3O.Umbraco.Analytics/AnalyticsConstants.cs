using NodaTime;

namespace N3O.Umbraco.Analytics;

public static class AnalyticsConstants {
    public static class Attribution {
        public const int DimensionsCount = 12;
        public static readonly Duration Lifetime = Duration.FromDays(3);
        
        public static class EventsCookie {
            public const string Name = "AttributionEvents";
        }
        
        public static class QueryString {
            public const string EncodedAttribution = "ea";
            public const string UtmCampaign = "utm_campaign";
            public const string UtmMedium = "utm_medium";
            public const string UtmSource = "utm_source";

            public static readonly string[] All = [UtmCampaign, UtmMedium, UtmSource];
        }

        public static string GetKey(int index) => $"d{index}";
    }

    public static class PageModuleKeys {
        public const string DataLayer = nameof(DataLayer);
        public const string GoogleAnalytics4 = nameof(GoogleAnalytics4);
        public const string GoogleTagManager = nameof(GoogleTagManager);
    }
}