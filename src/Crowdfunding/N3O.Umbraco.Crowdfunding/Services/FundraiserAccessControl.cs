using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Hosting;
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
    private readonly IContentLocator _contentLocator;
    private readonly IQueryStringAccessor _queryStringAccessor;

    public FundraiserAccessControl(IContentHelper contentHelper,
                                   IDataTypeService dataTypeService,
                                   ILookups lookups,
                                   IContentLocator contentLocator,
                                   IQueryStringAccessor queryStringAccessor,
                                   IMemberManager memberManager)
        : base(contentHelper, dataTypeService, memberManager) {
        _lookups = lookups;
        _contentLocator = contentLocator;
        _queryStringAccessor = queryStringAccessor;
    }

    protected override async Task<bool> AllowEditAsync(ContentProperties contentProperties) {
        var canEdit = await base.AllowEditAsync(contentProperties);
        
        if (canEdit) {
            canEdit = CanEdit(() => contentProperties.GetPropertyValueByAlias<string>(CrowdfundingConstants.Crowdfunder.Properties.Status));
        }

        return canEdit;
    }

    protected override async Task<bool> AllowEditAsync(IPublishedContent content) {
        var requestApiKey = _queryStringAccessor.GetValue(CrowdfundingConstants.Http.Headers.RequestApiKey);
        
        var canEdit = await base.AllowEditAsync(content) ||
                      UmbracoMemberOrApiKeyAuthorizeFilter.IsApiAuthorized(_contentLocator, requestApiKey);
        
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