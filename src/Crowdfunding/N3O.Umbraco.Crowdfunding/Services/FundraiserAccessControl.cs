using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Extensions;
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
    private readonly OurBackofficeUserAccessor _backofficeUserAccessor;

    public FundraiserAccessControl(IContentHelper contentHelper,
                                   IDataTypeService dataTypeService,
                                   ILookups lookups,
                                   IMemberManager memberManager,
                                   OurBackofficeUserAccessor backofficeUserAccessor)
        : base(contentHelper, dataTypeService, memberManager) {
        _lookups = lookups;
        _backofficeUserAccessor = backofficeUserAccessor;
    }

    protected override async Task<bool> AllowEditAsync(ContentProperties contentProperties) {
        var canEdit = await base.AllowEditAsync(contentProperties);
        
        if (canEdit) {
            canEdit = CanEdit(() => contentProperties.GetPropertyValueByAlias<string>(CrowdfundingConstants.Crowdfunder.Properties.Status));
        }

        return canEdit;
    }

    protected override async Task<bool> AllowEditAsync(IPublishedContent content) {
        var canEdit = await base.AllowEditAsync(content) || _backofficeUserAccessor.IsLoggedIntoBackOffice();
        
        if (canEdit) {
            canEdit = CanEdit(() => content.Value<string>(CrowdfundingConstants.Crowdfunder.Properties.Status));
        }

        return canEdit;
    }

    private bool CanEdit(Func<string> getValue) {
        var status = getValue().IfNotNull(_lookups.FindByName<CrowdfunderStatus>)?.SingleOrDefault();
        
        return status?.CanEdit != false;
    }

    protected override string ContentTypeAlias => CrowdfundingConstants.Fundraiser.Alias;
    protected override string PropertyAlias => CrowdfundingConstants.Fundraiser.Properties.Owner;
}