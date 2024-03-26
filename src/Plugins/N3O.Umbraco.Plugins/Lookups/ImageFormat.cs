using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Lookups;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Plugins.Lookups;

public class ImageFormat : Lookup {
    public ImageFormat(string id, string extension, IImageEncoder encoder) : base(id) {
        Extension = extension;
        Encoder = encoder;
    }
    
    public string Extension { get; }
    public IImageEncoder Encoder { get; }
    
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
    public static readonly ImageFormat Gif = new("gif", ".gif", new GifEncoder { SkipMetadata = true});
    public static readonly ImageFormat Jpg = new("jpg", ".jpg", new JpegEncoder { SkipMetadata = true, Quality = 80 });
    public static readonly ImageFormat Png = new("png", ".png", new PngEncoder { SkipMetadata = true, CompressionLevel = PngCompressionLevel.BestCompression });
    
    public static IEnumerable<ImageFormat> GetAllFormats() => StaticLookups.GetAll<ImageFormats, ImageFormat>().ToArray();
}
