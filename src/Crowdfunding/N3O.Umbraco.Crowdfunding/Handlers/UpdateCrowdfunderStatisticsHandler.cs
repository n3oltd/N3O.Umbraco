using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Mediator;
using NPoco;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public class UpdateCrowdfunderStatisticsHandler : IRequestHandler<UpdateCrowdfunderStatisticsCommand, None, None> {
    private readonly IUmbracoDatabaseFactory _umbracoDatabaseFactory;

    public UpdateCrowdfunderStatisticsHandler(IUmbracoDatabaseFactory umbracoDatabaseFactory) {
        _umbracoDatabaseFactory = umbracoDatabaseFactory;
    }

    public async Task<None> Handle(UpdateCrowdfunderStatisticsCommand req, CancellationToken cancellationToken) {
        var sql = GetUpdateCrowdfunderStatisticsSql(req.ContentId.Value);

        using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
            await db.ExecuteAsync(sql);
        }
        
        return None.Empty;
    }

    private Sql GetUpdateCrowdfunderStatisticsSql(Guid crowdfunderId) {
        var contributionsQuoteSumSql = Sql.Builder
                                          .Append($"SELECT SUM({nameof(Contribution.QuoteAmount)})")
                                          .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                                          .Where($"{nameof(Crowdfunder.Id)} = {crowdfunderId.ToString()}");
        
        var contributionsBaseSumSql = Sql.Builder
                                         .Append($"SELECT SUM({nameof(Contribution.BaseAmount)})")
                                         .From($"{CrowdfundingConstants.Tables.Contributions.Name}")
                                         .Where($"{nameof(Crowdfunder.Id)} = {crowdfunderId.ToString()}");
        
        var sql = Sql.Builder
                     .Append($"UPDATE {CrowdfundingConstants.Tables.Crowdfunders.Name} SET {nameof(Crowdfunder.ContributionsTotalQuote)} = ({contributionsQuoteSumSql.SQL}), {nameof(Crowdfunder.ContributionsTotalBase)} = ({contributionsBaseSumSql.SQL})");
        
        sql.Where($"{nameof(Crowdfunder.Id)} = {crowdfunderId.ToString()}");

        return sql;
    }
}