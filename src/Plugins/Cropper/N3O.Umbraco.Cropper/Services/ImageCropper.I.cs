using N3O.Umbraco.Cropper.DataTypes;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cropper;

public interface IImageCropper {
    Task CropAllAsync(CropperConfiguration configuration,
                      CropperSource source,
                      CancellationToken cancellationToken = default);

    public Task CropAsync(CropDefinition definition,
                          CropperSource.Crop crop,
                          CropperSource source,
                          CancellationToken cancellationToken = default);
}
