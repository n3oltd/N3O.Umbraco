using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.CrowdFunding.Lookups;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CropperValueReq : ValueReq {
    [Name("Storage Token")]
    public StorageToken StorageToken { get; set; }
    
    [Name("Crop Type")]
    public CropType CropType { get; set; }
    
    [Name("Circle Crop")]
    public CircleCropReq CircleCrop { get; set; }
    
    [Name("Rectangle Crop")]
    public RectangleCropReq RectangleCrop { get; set; }
    
    
    public override PropertyType Type => PropertyTypes.Cropper;
}