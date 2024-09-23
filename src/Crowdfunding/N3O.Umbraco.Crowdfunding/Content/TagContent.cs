using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Tags.Alias)]
public class TagContent : UmbracoContent<TagContent> {
    public string Name => Content().Name;
    public TagCategoryContent Category => GetPickedAs(x => x.Category);
}