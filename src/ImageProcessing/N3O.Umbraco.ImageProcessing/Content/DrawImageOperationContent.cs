using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.Extensions;
using N3O.Umbraco.Uploader.Models;

namespace N3O.Umbraco.ImageProcessing.Content;

public class DrawImageOperationContent : UmbracoElement<DrawImageOperationContent> {
    public FileUpload Image => this.GetFileUploadAs(x => x.Image);
    public float Opacity => GetValue(x => x.Opacity);
}
