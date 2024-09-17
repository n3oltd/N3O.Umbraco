using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Data.Models;

public class RectangleCropReq {
    [Name("Bottom Left")]
    public PointReq BottomLeft { get; set; }
    
    [Name("Top Right")]
    public PointReq TopRight { get; set; }
}