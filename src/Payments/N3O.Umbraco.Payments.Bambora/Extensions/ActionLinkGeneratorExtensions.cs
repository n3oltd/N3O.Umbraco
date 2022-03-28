using N3O.Umbraco.Entities;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Payments.Bambora.Controllers;

namespace N3O.Umbraco.Payments.Bambora.Extensions {
    public static class ActionLinkGeneratorExtensions {
        public static string GetPaymentThreeDSecureUrl(this IActionLinkGenerator actionLinkGenerator, EntityId flowId) {
            return actionLinkGenerator.GetUrl<BamboraController>(x => x.CompletePaymentThreeDSecureChallenge(null),
                                                                 new { flowId = flowId.ToString() });
        }
    }
}