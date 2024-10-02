using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace N3O.Umbraco.Cropper.Content;

public class CropBuilder : ICropBuilder {
    private readonly int _imageWidth;
    private readonly int _imageHeight;
    private int? _x;
    private int? _y;
    private int? _height;
    private int? _width;

    public CropBuilder(int imageWidth, int imageHeight) {
        _imageWidth = imageWidth;
        _imageHeight = imageHeight;
    }

    public ICropBuilder AutoCrop(CropDefinition cropDefinition) {
        return AutoCrop(cropDefinition.Width, cropDefinition.Height);
    }

    public ICropBuilder AutoCrop(int width, int height) {
        var aspectRatio = width / (decimal) height;
        var cropCandidates = new List<Rectangle>();
        
        cropCandidates.Add(new Rectangle(0, 0, _imageWidth, (int) (_imageWidth / aspectRatio)));
        cropCandidates.Add(new Rectangle(0, 0, (int) (_imageHeight * aspectRatio), _imageHeight));
        cropCandidates.Add(new Rectangle((int) Math.Max(0, (_imageWidth - width) / 2m),
                                         (int) Math.Max(0, (_imageHeight - height) / 2m),
                                         Math.Min(width, _imageWidth),
                                         Math.Min(height, _imageHeight)));


        var crop = cropCandidates.Where(r => r.X + r.Width <= _imageWidth && r.Y + r.Height <= _imageHeight)
                                 .MaxBy(x => x.Width * x.Height);

        CropTo(crop.X, crop.Y, crop.Width, crop.Height);

        return this;
    }

    public ICropBuilder CropTo(int x, int y, int width, int height) {
        _x = x;
        _y = y;
        _height = height;
        _width = width;

        return this;
    }

    public CropperSource.Crop Build() {
        Validate();
        
        var crop = new CropperSource.Crop();
        crop.Height = _height.GetValueOrThrow();
        crop.Width = _width.GetValueOrThrow();
        crop.X = _x.GetValueOrThrow();
        crop.Y = _y.GetValueOrThrow();

        return crop;
    }

    private void Validate() {
        if (_x == null || _y == null || _height == null || _width == null) {
            throw new Exception("Crop dimensions must be specified");
        }

        if ((_x + _width.Value) > _imageWidth || (_y + _height.Value) > _imageHeight) {
            throw new Exception("Crop dimensions are invalid");
        }
    }
}
