using N3O.Umbraco.Cropper.DataTypes;

namespace N3O.Umbraco.Data.Models;

public class CropperValueRes {
    public CropperSource Image { get; set; }
    public StorageToken StorageToken { get; set; }
    public CropperConfigurationRes Configuration { get; set; }
}