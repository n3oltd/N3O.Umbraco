using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;
using ElementType = N3O.Umbraco.Cloud.Platforms.Lookups.ElementType;

namespace N3O.Umbraco.Cloud.Platforms;

public class DonationButtonElementPreviewHtmlGenerator : ElementPreviewHtmlGenerator {
    public DonationButtonElementPreviewHtmlGenerator(ICdnClient cdnClient,
                                                     IJsonProvider jsonProvider,
                                                     IContentLocator contentLocator,
                                                     IUmbracoMapper mapper,
                                                     ILookups lookups,
                                                     IBaseCurrencyAccessor baseCurrencyAccessor)
        : base(cdnClient, jsonProvider, contentLocator, mapper, lookups, baseCurrencyAccessor) { }

    protected override ElementType ElementType => ElementTypes.DonationButton;
}