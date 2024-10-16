using N3O.Umbraco.Context;
using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.Queries;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class GetDashboardStatisticsHandler :
    IRequestHandler<GetDashboardStatisticsQuery, DashboardStatisticsCriteria, DashboardStatisticsRes> {
    private readonly Currency BaseCurrency;
    
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;
    private readonly IFormatter _formatter;
    

    public GetDashboardStatisticsHandler(IUmbracoDatabaseFactory umbracoDatabaseFactory,
                                         IBaseCurrencyAccessor baseCurrencyAccessor,
                                         IFormatter formatter) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
        _formatter = formatter;

        BaseCurrency = baseCurrencyAccessor.GetBaseCurrency();
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

            await Task.WhenAll(tasks);

            return res;
        }
    }

    private MoneyRes GetMoneyRes(decimal baseAmount) {
        var res = new MoneyRes();
        res.Amount = baseAmount;
        res.Currency = BaseCurrency;
        res.Text = _formatter.Number.FormatMoney(baseAmount, BaseCurrency);

        return res;
    }
}