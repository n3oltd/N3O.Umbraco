using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoPaymentMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<OpayoPayment, OpayoPaymentRes>((_, _) => new OpayoPaymentRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(OpayoPayment src, OpayoPaymentRes dest, MapperContext ctx) {
            if (src.RequireThreeDSecure) {
                var threeDSecure = new ThreeDSecure();
                threeDSecure.AcsTransId = src.AcsTransId;
                threeDSecure.ThreeDSecureUrl = src.ThreeDSecureUrl;
                threeDSecure.CReq = src.CReq;
                dest.ThreeDSecure = threeDSecure;
                dest.RequireThreeDSecure = src.RequireThreeDSecure;
            }
            
            dest.IsDeclined = src.IsDeclined;
            dest.DeclineReason = src.OpayoStatusDetail;
            dest.TransactionId = src.TransactionId;
            dest.OpayoErrorCode = src.OpayoErrorCode;
            dest.OpayoErrorMessage = src.OpayoErrorMessage;
        }
    }
}