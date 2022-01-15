using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Lookups;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace N3O.Umbraco.Plugins.Lookups {
    public class ImageFormat : Lookup {
        public ImageFormat(string id) : base(id) { }
        
        public static ImageFormat From(IImageFormat format) {
            if (format == JpegFormat.Instance) {
                return ImageFormats.Jpg;
            } else if (format == PngFormat.Instance) {
                return ImageFormats.Png;
            } else if (format == GifFormat.Instance) {
                return ImageFormats.Gif;
            } else {
                throw UnrecognisedValueException.For(format);
            }
        }
    }

    public class ImageFormats : StaticLookupsCollection<ImageFormat> {
        public static readonly ImageFormat Gif = new("gif");
        public static readonly ImageFormat Jpg = new("jpg");
        public static readonly ImageFormat Png = new("png");
    }
}
