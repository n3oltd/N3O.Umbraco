using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class CropperShapeExtensions {
    public static int GetHeight(int? topRightY, int? bottomLeftY) {
        var height = topRightY.GetValueOrThrow() - bottomLeftY.GetValueOrThrow();

        return height;
    }
    
    public static int GetWidth(int? topRightX, int? bottomLeftX) {
        var width = topRightX.GetValueOrThrow() - bottomLeftX.GetValueOrThrow();

        return width;
    }
    
}