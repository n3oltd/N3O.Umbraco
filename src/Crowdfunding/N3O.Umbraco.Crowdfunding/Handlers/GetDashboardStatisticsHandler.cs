using Microsoft.Extensions.Logging;
using N3O.Umbraco.Context;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Mediator;
using NPoco;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler :
    IRequestHandler<GetDashboardStatisticsQuery, DashboardStatisticsCriteria, DashboardStatisticsRes> {
    private readonly Currency _baseCurrency;
    
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IUmbracoMapper _umbracoMapper;
    private readonly ILogger<GetDashboardStatisticsHandler> _logger;

    public GetDashboardStatisticsHandler(IUmbracoDatabaseFactory umbracoDatabaseFactory,
                                         IBaseCurrencyAccessor baseCurrencyAccessor,
                                         IUmbracoMapper mapper,
                                         ILogger<GetDashboardStatisticsHandler> logger) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _umbracoMapper = mapper;
        _logger = logger;
        _baseCurrency = baseCurrencyAccessor.GetBaseCurrency();
    }
    
    public async Task<DashboardStatisticsRes> Handle(GetDashboardStatisticsQuery req,
                                                     CancellationToken cancellationToken) {
        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            var res = new DashboardStatisticsRes();
            
            var tasks = new List<Task>();
            
            tasks.Add(PopulateAllocationsAsync(db, req.Model, res));
            tasks.Add(PopulateCampaignsAsync(db, req.Model, res));
            tasks.Add(PopulateContributionsAsync(db, req.Model, res));
            tasks.Add(PopulateFundraisersAsync(db, req.Model, res));

            PopulateBaseCurrency(res);

            await Task.WhenAll(tasks);

            return res;
        }
    }

    private void PopulateBaseCurrency(DashboardStatisticsRes res) {
        res.BaseCurrency = _umbracoMapper.Map<Currency, CurrencyRes>(_baseCurrency);
    }

    private MoneyRes GetMoneyRes(decimal baseAmount) {
        var money = new Money(baseAmount, _baseCurrency);

        return _umbracoMapper.Map<Money, MoneyRes>(money);
    }

    private void LogQuery(Sql sql) {
        _logger.Log(LogLevel.Information, "Executing crowdfuding statistics query: {Sql}", sql.SQL);
    }
}