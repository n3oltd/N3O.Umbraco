using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Json;
using System;
using System.Collections.Generic;
using DesignationType = N3O.Umbraco.Cloud.Platforms.Lookups.DesignationType;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class DesignationPreviewTagGenerator : PreviewTagGenerator {
    protected DesignationPreviewTagGenerator(ICdnClient cdnClient, IJsonProvider jsonProvider)
        : base(cdnClient, jsonProvider) { }
    
    protected abstract DesignationType DesignationType { get; }

    protected override string ContentTypeAlias => DesignationType.ContentTypeAlias;
    protected override string TagName => ElementTypes.DonationForm.TagName;

    protected override void PopulatePreviewData(IReadOnlyDictionary<string, object> content,
                                                Dictionary<string, object> previewData) {
        var publishedDesignation = new PublishedDesignation();

        // TODO Populate here common properties
        PopulatePublishedDesignation(content, publishedDesignation);
        
        var publishedDonationForm = new PublishedDonationForm();
        publishedDonationForm.Id = Guid.NewGuid().ToString();
        publishedDonationForm.Designation = publishedDesignation;

        previewData["publishedForm"] = publishedDonationForm;
    }
    
    protected abstract void PopulatePublishedDesignation(IReadOnlyDictionary<string, object> content,
                                                         PublishedDesignation publishedDesignation);
}