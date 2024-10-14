using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.Extensions;
using N3O.Umbraco.Uploader.Models;

namespace N3O.Umbraco.Crowdfunding.Content;

public class TagCategoryContent : UmbracoContent<TagCategoryContent> {
    public FileUpload Icon => this.GetFileUploadAs(x => x.Icon);
}