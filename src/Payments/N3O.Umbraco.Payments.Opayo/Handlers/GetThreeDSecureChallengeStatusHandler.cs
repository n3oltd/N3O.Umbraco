using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Extensions;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class GetThreeDSecureChallengeStatusHandler :
        IRequestHandler<GetThreeDSecureChallengeStatusCommand, string, ThreeDSecureStatus> {
        private readonly IOpayoClient _opayoClient;

        public GetThreeDSecureChallengeStatusHandler(IOpayoClient opayoClient) {
            _opayoClient = opayoClient;
        }
        
        public async Task<ThreeDSecureStatus> Handle(GetThreeDSecureChallengeStatusCommand req,
                                                     CancellationToken cancellationToken) {
            var transaction = await _opayoClient.GetTransactionByIdAsync(req.Model);

            var res = new ThreeDSecureStatus();
            res.Completed = !transaction.ThreeDSecure.Status.Equals("NotChecked");
            res.Success = transaction.IsSuccess();

            return res;
        }
    }
}