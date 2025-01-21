using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.TaxRelief.Content;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class TaxReliefSettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<TaxReliefSettingsContent, TaxReliefSettings>((_, _) => new TaxReliefSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(TaxReliefSettingsContent src, TaxReliefSettings dest, MapperContext ctx) {
        dest.Name = src.Scheme.Name;
        dest.SchemeId = src.Scheme.Id;
        dest.Type = (ProcessorType) Enum.Parse(typeof(ProcessorType), src.Scheme.Type.Id, true);
        dest.Identifier = "v1_4years";
    }
}