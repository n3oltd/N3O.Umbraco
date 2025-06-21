using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedPaymentTermsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PaymentTermsContent, PublishedPaymentTerms>((_, _) => new PublishedPaymentTerms(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PaymentTermsContent src, PublishedPaymentTerms dest, MapperContext ctx) {
        dest.Text = src.Text;
        dest.Url = src.Link.GetPublishedUri();
    }
}