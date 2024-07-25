using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Crowdfunding.Models;

public class PointReq {
    [Name("X")]
    public int? X { get; set; }
    
    [Name("Y")]
    public int? Y { get; set; }
}