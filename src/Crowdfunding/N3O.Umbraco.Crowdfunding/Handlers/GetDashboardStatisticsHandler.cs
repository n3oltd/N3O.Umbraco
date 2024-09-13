using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class GetDashboardStatisticsHandler :
    IRequestHandler<GetDashboardStatisticsQuery, DashboardStatisticsCriteria, DashboardStatisticsRes> {
    public Task<DashboardStatisticsRes> Handle(GetDashboardStatisticsQuery request, CancellationToken cancellationToken) {
        throw new System.NotImplementedException();
    }
}