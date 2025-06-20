using MuslimHands.Website.Connect.Clients;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models.Connect.PaymentTerms;

public class PublishedPaymentTermsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PlatformsPaymentTerms, PublishedPaymentTerms>((_, _) => new PublishedPaymentTerms(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PlatformsPaymentTerms src, PublishedPaymentTerms dest, MapperContext ctx) {
        var termsUrl = src.Link.Content?.AbsoluteUrl() ?? src.Link.Url;
        
        dest.Text = src.Text;
        dest.Url = new Uri(termsUrl);
    }
}