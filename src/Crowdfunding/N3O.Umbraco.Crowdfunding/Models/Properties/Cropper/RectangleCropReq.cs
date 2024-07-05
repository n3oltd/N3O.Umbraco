using N3O.Umbraco.Attributes;
using System.Drawing;

namespace N3O.Umbraco.Crowdfunding.Models;

public class RectangleCropReq {
    [Name("Bottom Left")]
    public Point? BottomLeft { get; set; }
    
    [Name("Top Right")]
    public Point? TopRight { get; set; }
}