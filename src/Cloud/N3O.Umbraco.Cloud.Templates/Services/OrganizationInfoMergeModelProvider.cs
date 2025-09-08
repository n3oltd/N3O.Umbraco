using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Templates;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Templates;

public class OrganizationInfoMergeModelProvider : MergeModelProvider<IOrganizationInfo> {
    private readonly IOrganizationInfoAccessor _organizationInfoAccessor;

    public OrganizationInfoMergeModelProvider(IOrganizationInfoAccessor organizationInfoAccessor) {
        _organizationInfoAccessor = organizationInfoAccessor;
    }
    
    protected override async Task<IOrganizationInfo> GetModelAsync(IPublishedContent content,
                                                                   CancellationToken cancellationToken) {
        var organizationInfo = await _organizationInfoAccessor.GetOrganizationInfoAsync(cancellationToken);
        
        return organizationInfo;
    }
    
    public override string Key => "our_organization";
}