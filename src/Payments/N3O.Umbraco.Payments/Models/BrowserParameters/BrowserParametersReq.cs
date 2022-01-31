using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Payments.Models {
    public class BrowserParametersReq {
        [Name("Colour Depth")]
        public int? ColourDepth { get; set; }

        [Name("Java Enabled")]
        public bool? JavaEnabled { get; set; }

        [Name("JavaScript Enabled")]
        public bool? JavaScriptEnabled { get; set; }
        
        [Name("Screen Height")]
        public int? ScreenHeight { get; set; }
        
        [Name("Screen Width")]
        public int? ScreenWidth { get; set; }

        [Name("UTC Offset Minutes")]
        public int? UtcOffsetMinutes { get; set; }
        
        public int GetColourDepth() {
            var allowed = new[] { 1, 4, 8, 15, 16, 24, 32, 48 };
            var colourDepth = allowed.GetClosetItem(x => ColourDepth.GetValueOrThrow() - x, x => x >= 0);

            return colourDepth;
        }
    }
}