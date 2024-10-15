using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class RecalculateContributionTotalsHandler : IRequestHandler<RecalculateContributionTotalsCommand, None, None> {
    private readonly ICrowdfunderRepository _crowdfunderRepository;

    public RecalculateContributionTotalsHandler(ICrowdfunderRepository crowdfunderRepository) {
        _crowdfunderRepository = crowdfunderRepository;
    }

    public async Task<None> Handle(RecalculateContributionTotalsCommand req, CancellationToken cancellationToken) {
        await _crowdfunderRepository.RecalculateContributionsTotalAsync(req.ContentId.Value);
        
        return None.Empty;
    }
}