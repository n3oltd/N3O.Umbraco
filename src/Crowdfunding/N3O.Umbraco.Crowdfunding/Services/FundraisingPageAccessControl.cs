using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding; 

public class FundraisingPageAccessControl : IContentAccessControl {
    private readonly IContentHelper _contentHelper;

    public FundraisingPageAccessControl(IContentHelper contentHelper) {
        _contentHelper = contentHelper;
    }
    
    public bool CanApply(IContent content) {
        return content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.CrowdfundingPage.Alias);
    }

    public bool CanEdit(IMember member, IContent content) {
        var contentProperties = _contentHelper.GetContentProperties(content);

        var allowedMembers = _contentHelper.GetPickerValues<IPublishedContent>(contentProperties, CrowdfundingConstants.CrowdfundingPage.Properties.AllowedMembers);

        if (allowedMembers.Any(x => x.Id == member.Id)) {
            return true;
        }

        return false;
    }
}