using N3O.Umbraco.Extensions;
using N3O.Umbraco.ImageProcessing.Models;
using SixLabors.ImageSharp;

namespace N3O.Umbraco.ImageProcessing.Extensions;

public static class HoldSizeExtensions {
    public static Size GetSize(this IHoldSize obj) {
        return new Size(obj.Width.GetValueOrThrow(), obj.Height.GetValueOrThrow());
    }
    
    public static bool HasSize(this IHoldSize obj) {
        return obj.Height > 0 && obj.Width > 0;
    }
}