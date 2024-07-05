using System;

namespace N3O.Umbraco.Analytics;

public static class AnalyticsConstants {
    public static class Attribution {
        public static class Cookie {
            public const string Name = "n3o_attribution";
            public static readonly TimeSpan Lifetime = TimeSpan.FromDays(90);
        }

        public static string GetKey(int index) => $"d{index}";
    }

    public static class PageModuleKeys {
        public const string DataLayer = nameof(DataLayer);
        public const string GoogleAnalytics4 = nameof(GoogleAnalytics4);
        public const string GoogleTagManager = nameof(GoogleTagManager);
    }
}