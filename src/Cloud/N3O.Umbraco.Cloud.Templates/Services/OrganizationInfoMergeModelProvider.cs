using N3O.Umbraco.Templates;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Templates;

public class OrganizationInfoMergeModelProvider : MergeModelsProvider {
    private readonly IOrganizationInfoAccessor _organizationInfoAccessor;

    public OrganizationInfoMergeModelProvider(IOrganizationInfoAccessor organizationInfoAccessor) {
        _organizationInfoAccessor = organizationInfoAccessor;
    }

    protected override async Task PopulateModelsAsync(IPublishedContent content,
                                                      Dictionary<string, object> mergeModels,
                                                      CancellationToken cancellationToken = default) {
        mergeModels["our_organization"] = await _organizationInfoAccessor.GetOrganizationInfoAsync(cancellationToken);
    }
}