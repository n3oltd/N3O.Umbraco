using N3O.Umbraco.Attributes;
using System.Drawing;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CircleCropReq {
    [Name("Center")]
    public PointReq Center { get; set; }
    
    [Name("Radius")]
    public int? Radius { get; set; }
}