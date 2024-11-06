using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.Extensions;
using N3O.Umbraco.Uploader.Models;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Settings.HomePageTemplate.Alias)]
public class HomePageTemplateContent : UmbracoContent<HomePageTemplateContent> {
    public FileUpload SearchBackgroundImage => this.GetFileUploadAs(x => x.SearchBackgroundImage);
    public FileUpload SearchSideImage => this.GetFileUploadAs(x => x.SearchSideImage);
}