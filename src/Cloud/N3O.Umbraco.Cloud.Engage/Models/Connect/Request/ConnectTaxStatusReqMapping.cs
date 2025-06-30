using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.TaxRelief.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class ConnectTaxStatusReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<TaxStatus, ConnectTaxStatusReq>((_, _) => new ConnectTaxStatusReq(), Map);
    }

    // Umbraco.Code.MapAll -TermsIdentifier
    private void Map(TaxStatus src, ConnectTaxStatusReq dest, MapperContext ctx) {
        dest.CanClaim = src.ToBool();
    }
}