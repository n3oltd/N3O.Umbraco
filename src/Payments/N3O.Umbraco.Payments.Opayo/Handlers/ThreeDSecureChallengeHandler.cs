using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class ThreeDSecureChallengeHandler :
        IRequestHandler<ThreeDSecureChallengeCommand, ThreeDSecureChallengeReq, None> {
        private readonly IOpayoClient _opayoClient;

        public ThreeDSecureChallengeHandler(IOpayoClient opayoClient) {
            _opayoClient = opayoClient;
        }

        public async Task<None> Handle(ThreeDSecureChallengeCommand req, CancellationToken cancellationToken) {
            var apiReq = new ApiThreeDSecureChallenge();
            apiReq.CRes = req.Model.CRes;
            apiReq.CRes = req.Model.ThreeDsSessionData;
            
            var transaction = await _opayoClient.ThreeDSecureChallengeAsync(apiReq);

            return None.Empty;
        }
    }
}