using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Models {
    public class CardPaymentMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<CardPayment, CardPaymentRes>((_, _) => new CardPaymentRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(CardPayment src, CardPaymentRes dest, MapperContext ctx) {
            dest.ThreeDSecureRequired = src.ThreeDSecureRequired;
            dest.ThreeDSecureCompleted = src.ThreeDSecureCompleted;
            dest.ThreeDSecureUrl = src.ThreeDSecureUrl;
            dest.Fallback = src.Fallback;
            dest.Challenge = src.Challenge;
        }
    }
}