using N3O.Umbraco.Attributes;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Payments.Models {
    public class BrowserParametersReq {
        [Name("Screen Width")]
        public int ScreenWidth { get; set; }

        [Name("Screen Height")]
        public int ScreenHeight { get; set; }

        [Name("Color")]
        public int? ColorDepth { get; set; }

        [Name("Java Enabled")]
        public bool? JavaEnabled { get; set; }

        [Name("JavaScript Enabled")]
        public bool? JavaScriptEnabled { get; set; }

        [Name("Timezone")]
        public Timezone Timezone { get; set; }
    }
}