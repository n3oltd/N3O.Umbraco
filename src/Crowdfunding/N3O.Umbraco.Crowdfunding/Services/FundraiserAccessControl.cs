using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Lookups;
using System;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Crowdfunding;

public class FundraiserAccessControl : MembersAccessControl {
    private readonly ILookups _lookups;
    
    public FundraiserAccessControl(IContentHelper contentHelper,
                                   IDataTypeService dataTypeService,
                                   ILookups lookups,
                                   IMemberManager memberManager)
        : base(contentHelper, dataTypeService, memberManager) {
        _lookups = lookups;
    }

    protected override async Task<bool> AllowEditAsync(ContentProperties contentProperties) {
        var canEdit = await base.AllowEditAsync(contentProperties);
        
        if (canEdit) {
            var status = GetFundraiserStatus(() => contentProperties.GetPropertyValueByAlias<string>(CrowdfundingConstants.Crowdfunder.Properties.Status));
        
            canEdit = status != CrowdfunderStatuses.Ended;
        }

        return canEdit;
    }

    protected override async Task<bool> AllowEditAsync(IPublishedContent content) {
        var canEdit = await base.AllowEditAsync(content);
        
        if (canEdit) {
            var status = GetFundraiserStatus(() => content.Value<string>(CrowdfundingConstants.Crowdfunder.Properties.Status));
            
            canEdit = status != CrowdfunderStatuses.Ended;
        }

        return canEdit;
    }
    
    private CrowdfunderStatus GetFundraiserStatus(Func<string> getValue) {
        return _lookups.FindByName<CrowdfunderStatus>(getValue()).Single();
    }

    protected override string ContentTypeAlias => CrowdfundingConstants.Fundraiser.Alias;
    protected override string PropertyAlias => CrowdfundingConstants.Fundraiser.Properties.Owner;
}