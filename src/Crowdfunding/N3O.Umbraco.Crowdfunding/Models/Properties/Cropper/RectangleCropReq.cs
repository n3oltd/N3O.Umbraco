using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Crowdfunding.Models;

public class RectangleCropReq {
    [Name("Bottom Left")]
    public PointReq BottomLeft { get; set; }
    
    [Name("Top Right")]
    public PointReq TopRight { get; set; }
}