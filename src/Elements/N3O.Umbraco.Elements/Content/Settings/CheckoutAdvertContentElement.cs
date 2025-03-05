using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.Extensions;
using N3O.Umbraco.Uploader.Models;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Elements.Content;

public class CheckoutAdvertContentElement : UmbracoElement<CheckoutAdvertContentElement> {
    public FileUpload Image => this.GetFileUploadAs(x => x.Image);
    public Link Link => GetValue(x => x.Link);
}