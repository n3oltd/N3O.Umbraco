using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Json;
using System;
using System.Collections.Generic;
using ElementType = N3O.Umbraco.Cloud.Platforms.Lookups.ElementType;

namespace N3O.Umbraco.Cloud.Platforms;

public class DonationFormElementPreviewTagGenerator : ElementPreviewTagGenerator {
    public DonationFormElementPreviewTagGenerator(ICdnClient cdnClient, IJsonProvider jsonProvider)
        : base(cdnClient, jsonProvider) { }

    protected override ElementType ElementType => ElementTypes.DonationForm;
    
    protected override void PopulatePreviewData(IReadOnlyDictionary<string, object> content,
                                                Dictionary<string, object> previewData) {
        var publishedDonationForm = new PublishedDonationForm();
        publishedDonationForm.Id = Guid.NewGuid().ToString();
        // TODO

        previewData["publishedForm"] = publishedDonationForm;
    }
}