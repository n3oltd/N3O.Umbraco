using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class CropperPropertyType : PropertyType<CropperValueReq> {
    public CropperPropertyType() : base("cropper") { }

    protected override async Task UpdatePropertyAsync(IContentPublisher contentPublisher, string alias, CropperValueReq data) {
        var cropperPropertyBuilder = contentPublisher.Content.Cropper(alias);
        
        await cropperPropertyBuilder.SetImageAsync(data.StorageToken);

        cropperPropertyBuilder.AddCrop();
    }
}