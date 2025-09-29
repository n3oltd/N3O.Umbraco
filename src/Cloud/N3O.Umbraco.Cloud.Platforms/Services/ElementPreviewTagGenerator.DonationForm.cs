using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Json;
using N3O.Umbraco.Markup;
using Umbraco.Cms.Core.Mapping;
using ElementType = N3O.Umbraco.Cloud.Platforms.Lookups.ElementType;

namespace N3O.Umbraco.Cloud.Platforms;

public class DonationFormElementPreviewTagGenerator : ElementPreviewTagGenerator {
    public DonationFormElementPreviewTagGenerator(ICdnClient cdnClient,
                                                  IJsonProvider jsonProvider,
                                                  IContentLocator contentLocator,
                                                  IUmbracoMapper mapper,
                                                  IMarkupEngine markupEngine) 
        : base(cdnClient, jsonProvider, contentLocator, mapper, markupEngine) { }

    protected override ElementType ElementType => ElementTypes.DonationForm;
}