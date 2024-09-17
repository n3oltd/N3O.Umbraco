using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Data.Models;

public class CircleCropReq {
    [Name("Center")]
    public PointReq Center { get; set; }
    
    [Name("Radius")]
    public int? Radius { get; set; }
}