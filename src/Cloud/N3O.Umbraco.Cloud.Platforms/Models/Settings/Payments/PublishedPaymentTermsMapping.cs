using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models.Connect.PaymentTerms;

public class PublishedPaymentTermsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PaymentTermsContent, PublishedPaymentTerms>((_, _) => new PublishedPaymentTerms(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PaymentTermsContent src, PublishedPaymentTerms dest, MapperContext ctx) {
        var termsUrl = src.Link.Content?.AbsoluteUrl() ?? src.Link.Url;
        
        dest.Text = src.Text;
        dest.Url = new Uri(termsUrl);
    }
}