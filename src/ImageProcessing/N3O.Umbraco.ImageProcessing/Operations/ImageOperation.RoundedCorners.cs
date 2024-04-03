using N3O.Umbraco.ImageProcessing.Content;
using N3O.Umbraco.ImageProcessing.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Umbraco.Cms.Core.IO;

namespace N3O.Umbraco.ImageProcessing.Operations;

public class RoundedCornersOperation : ImageOperation<RoundedCornersOperationElement> {
    public RoundedCornersOperation(MediaFileManager mediaFileManager) : base(mediaFileManager) { }

    protected override void Apply(RoundedCornersOperationElement options, IImageProcessingContext image) {
        if (options.HasSize()) {
            image = image.Resize(options.GetSize());
        }

        ApplyRoundedCorners(image, options.CornerRadius);
    }
    
    private void ApplyRoundedCorners(IImageProcessingContext image, int cornerRadius) {
        var size = image.GetCurrentSize();
        var corners = BuildCorners(size.Width, size.Height, cornerRadius);

        image.SetGraphicsOptions(new GraphicsOptions {
            Antialias = true,
            // Enforces that any part of this shape that has color is punched out of the background
            AlphaCompositionMode = PixelAlphaCompositionMode.DestOut
        });

        // Mutating in here as we already have a cloned original
        // use any color (not Transparent), so the corners will be clipped
        foreach (IPath path in corners) {
            image = image.Fill(Color.Red, path);
        }
    }
    
    private IPathCollection BuildCorners(int imageWidth, int imageHeight, int cornerRadius) {
        // First create a square
        var rect = new RectangularPolygon(-0.5f, -0.5f, cornerRadius, cornerRadius);

        // Then cut out of the square a circle so we are left with a corner
        var cornerTopLeft = rect.Clip(new EllipsePolygon(cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius));

        // Corner is now a corner shape positions top left
        // let's make 3 more positioned correctly, we can do that by translating the original around the center of the image

        var rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
        var bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;

        // Move it across the width of the image - the width of the shape
        var cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
        var cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
        var cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

        return new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);
    }
}