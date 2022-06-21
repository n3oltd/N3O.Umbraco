using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Models;

public class CardPaymentMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CardPayment, CardPaymentRes>((_, _) => new CardPaymentRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(CardPayment src, CardPaymentRes dest, MapperContext ctx) {
        dest.ThreeDSecureRequired = src.ThreeDSecureRequired;
        dest.ThreeDSecureCompleted = src.ThreeDSecureCompleted;
        dest.ThreeDSecureV1 = src.ThreeDSecureV1;
        dest.ThreeDSecureV2 = src.ThreeDSecureV2;
    }
}
