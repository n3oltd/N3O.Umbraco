using N3O.Umbraco.Attributes;
using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Data.Models;

public class CropperValueReq : ValueReq {
    [Name("Storage Token")]
    public StorageToken StorageToken { get; set; }
    
    [Name("Crop Type")]
    public CropShape Shape { get; set; }
    
    [Name("Circle")]
    public CircleCropReq Circle { get; set; }
    
    [Name("Rectangle")]
    public RectangleCropReq Rectangle { get; set; }
    
    public override PropertyType Type => PropertyTypes.Cropper;
}