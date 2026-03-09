using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Lookups;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Plugins.Lookups;

public class ImageFormat : Lookup {
    public ImageFormat(string id, string extension, string contentType, IImageEncoder encoder) : base(id) {
        Extension = extension;
        ContentType = contentType;
        Encoder = encoder;
    }
    
    public string Extension { get; }
    public string ContentType { get; }
    public IImageEncoder Encoder { get; }
    
    public static ImageFormat From(IImageFormat format) {
        if (format == JpegFormat.Instance) {
            return ImageFormats.Jpg;
        } else if (format == PngFormat.Instance) {
            return ImageFormats.Png;
        } else if (format == GifFormat.Instance) {
            return ImageFormats.Gif;
        } else if (format == WebpFormat.Instance) {
            return ImageFormats.Webp;
        } else {
            throw UnrecognisedValueException.For(format);
        }
    }
}

public class ImageFormats : StaticLookupsCollection<ImageFormat> {
    public static readonly ImageFormat Gif = new("gif", ".gif", "image/gif", new GifEncoder { SkipMetadata = true});
    public static readonly ImageFormat Jpg = new("jpg", ".jpg", "image/jpeg", new JpegEncoder { SkipMetadata = true, Quality = 80 });
    public static readonly ImageFormat Png = new("png", ".png", "image/png", new PngEncoder { SkipMetadata = true, CompressionLevel = PngCompressionLevel.BestCompression });
    public static readonly ImageFormat Webp = new("webp", ".webp", "image/webp", new WebpEncoder { SkipMetadata = true, Quality = 80 });
    
    public static IEnumerable<ImageFormat> GetAllFormats() => StaticLookups.GetAll<ImageFormats, ImageFormat>().ToArray();
}
