using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.ImageProcessing.Models;
using N3O.Umbraco.Uploader.Extensions;
using N3O.Umbraco.Uploader.Models;

namespace N3O.Umbraco.ImageProcessing.Content;

public class DrawImageOperationElement : UmbracoElement<DrawImageOperationElement>, IHoldSize, IHoldPoint {
    public int? Height => GetValue(x => x.Height);
    public FileUpload Image => this.GetFileUploadAs(x => x.Image);
    public int Opacity => GetValue(x => x.Opacity);
    public int? Width => GetValue(x => x.Width);
    [UmbracoProperty("pointX")] public int? X => GetValue(x => x.X);
    [UmbracoProperty("pointY")] public int? Y => GetValue(x => x.Y);
}
