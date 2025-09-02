using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Json;
using System.Collections.Generic;
using DesignationType = N3O.Umbraco.Cloud.Platforms.Lookups.DesignationType;

namespace N3O.Umbraco.Cloud.Platforms;

public class FundDesignationPreviewTagGenerator : DesignationPreviewTagGenerator {
    public FundDesignationPreviewTagGenerator(ICdnClient cdnClient, IJsonProvider jsonProvider)
        : base(cdnClient, jsonProvider) { }
    
    protected override DesignationType DesignationType => DesignationTypes.Fund;
    
    protected override void PopulatePublishedDesignation(IReadOnlyDictionary<string, object> content,
                                                         PublishedDesignation publishedDesignation) {
        var publishedFundDesignation = new PublishedFundDesignation();
        
        // TODO Make sure to factor common code in these classes and the code used in the notifications when publishing
        
        publishedDesignation.Fund = publishedFundDesignation;
    }
}