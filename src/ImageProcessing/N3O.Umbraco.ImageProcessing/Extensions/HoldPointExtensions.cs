using N3O.Umbraco.Extensions;
using N3O.Umbraco.ImageProcessing.Models;
using SixLabors.ImageSharp;

namespace N3O.Umbraco.ImageProcessing.Extensions;

public static class HoldPointExtensions {
    public static Point GetPoint(this IHoldPoint obj) {
        return new Point(obj.X.GetValueOrThrow(), obj.Y.GetValueOrThrow());
    }
    
    public static bool HasPoint(this IHoldPoint obj) {
        return obj.X.HasValue() && obj.Y.HasValue();
    }
}