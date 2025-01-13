using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;

namespace N3O.Umbraco.Crowdfunding;

public class RemotePagesApp : IContentAppFactory {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public RemotePagesApp(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups) {
        if (_webHostEnvironment.IsProduction()) {
            return null;
        }
        
        var content = source as IContent;

        if (content == null || content.Id == default) {
            return null;
        }

        if (!content.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Fundraisers.Alias)) {
            return null;
        }

        return new ContentApp {
            Alias = "crowdfundingRemotePages",
            Name = "Live",
            Icon = "icon-sitemap",
            View = "/App_Plugins/N3O.Umbraco.Crowdfunding.RemotePages/N3O.Umbraco.Crowdfunding.RemotePages.html",
            Weight = -999
        };
    }
}