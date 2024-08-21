using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper;
using N3O.Umbraco.Cropper.Content;
using N3O.Umbraco.Cropper.Extensions;
using N3O.Umbraco.CrowdFunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class CropperPropertyType : PropertyType<CropperValueReq> {
    public CropperPropertyType()
        : base("cropper",
               (ctx, src, dest) => dest.Cropper = ctx.Map<PublishedContentProperty, CropperValueRes>(src),
               CropperConstants.PropertyEditorAlias) { }

    protected override async Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                      string alias,
                                                      CropperValueReq data) {
        var cropper = contentBuilder.Cropper(alias);
        
        await cropper.SetImageAsync(data.StorageToken);

        if (data.Shape == CropShapes.Circle) {
            AddCircleCrop(cropper, data.Circle);
        } else if (data.Shape == CropShapes.Rectangle) {
            AddRectangleCrop(cropper, data.Rectangle);
        } else {
            throw UnrecognisedValueException.For(data.Shape);
        }
    }

    private void AddCircleCrop(CropperPropertyBuilder cropBuilder, CircleCropReq circleCrop) {
        var center = circleCrop.Center;
        var radius = circleCrop.Radius.GetValueOrThrow();
            
        cropBuilder.AddCrop()
                   .CropTo(center.X.GetValueOrThrow() - radius,
                           center.Y.GetValueOrThrow() - radius,
                           radius * 2,
                           radius * 2);
    }

    private void AddRectangleCrop(CropperPropertyBuilder cropBuilder, RectangleCropReq rectangleCrop) {
        var topRight = rectangleCrop.TopRight;
        var bottomLeft = rectangleCrop.BottomLeft;
            
        var width = topRight.X.GetValueOrThrow() - bottomLeft.X.GetValueOrThrow();
        var height = topRight.Y.GetValueOrThrow() - bottomLeft.Y.GetValueOrThrow();
            
        cropBuilder.AddCrop()
                   .CropTo(bottomLeft.X.GetValueOrThrow(),
                           bottomLeft.Y.GetValueOrThrow(),
                           width,
                           height);
    }
}