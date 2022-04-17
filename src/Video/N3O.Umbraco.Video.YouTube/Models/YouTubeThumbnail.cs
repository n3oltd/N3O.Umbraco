using System.Drawing;

namespace N3O.Umbraco.Video.YouTube.Models {
    public class YouTubeThumbnail : Value {
        public YouTubeThumbnail(Size size, string url) {
            Size = size;
            Url = url;
        }

        public Size Size { get; }
        public string Url { get; }
    }
}