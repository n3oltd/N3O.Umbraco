using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;

namespace N3O.Umbraco.Data.Models;

public class RectangleCropReq {
    [Name("Bottom Left")]
    public PointReq BottomLeft { get; set; }
    
    [Name("Top Right")]
    public PointReq TopRight { get; set; }
    
    [JsonIgnore]
    public int Height => TopRight.Y.GetValueOrThrow() - BottomLeft.Y.GetValueOrThrow();
    
    [JsonIgnore]
    public int Width => TopRight.X.GetValueOrThrow() - BottomLeft.X.GetValueOrThrow();
}