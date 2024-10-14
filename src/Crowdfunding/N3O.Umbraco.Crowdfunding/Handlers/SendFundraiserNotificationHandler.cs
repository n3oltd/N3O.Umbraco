using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class SendFundraiserNotificationHandler :
    IRequestHandler<SendFundraiserNotificationCommand, FundraiserNotificationReq, None> {
    public async Task<None> Handle(SendFundraiserNotificationCommand req, CancellationToken cancellationToken) {
        throw new System.NotImplementedException();
    }
}