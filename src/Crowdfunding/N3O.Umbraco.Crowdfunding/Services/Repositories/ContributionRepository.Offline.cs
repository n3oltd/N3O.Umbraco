using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using NodaTime;
using NodaTime.Extensions;
using NPoco;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Crowdfunding;

public partial class ContributionRepository {
    public async Task AddOfflineContributionAsync(string transactionReference,
                                                  LocalDate date,
                                                  ICrowdfunderInfo crowdfunderInfo,
                                                  string email,
                                                  string name,
                                                  bool anonymous,
                                                  bool taxRelief,
                                                  string fundDimension1,
                                                  string fundDimension2,
                                                  string fundDimension3,
                                                  string fundDimension4,
                                                  Money value,
                                                  GivingType givingType) {
        var timestamp = date.ToDateTimeUnspecified().ToInstant();
        
        var contribution = await GetContributionAsync(ContributionType.Offline,
                                                      crowdfunderInfo.Type,
                                                      crowdfunderInfo.Id,
                                                      transactionReference,
                                                      timestamp,
                                                      email,
                                                      name,
                                                      anonymous,
                                                      null,
                                                      taxRelief,
                                                      fundDimension1,
                                                      fundDimension2,
                                                      fundDimension3,
                                                      fundDimension4,
                                                      givingType,
                                                      value,
                                                      null);
        
        _toCommit.Add(contribution);
    }

    public async Task CommitOfflineDonationsAsync() {
        if (_toCommit.Any()) {
            using (var db = _umbracoDatabaseFactory.CreateDatabase()) {
                await RemoveExisitingOfflineDonationsAsync(db);
                await AddOfflineDonationsAsync(db);
            }
        }
        
        _toCommit.Clear();
    }
    
    private async Task AddOfflineDonationsAsync(IUmbracoDatabase db) {
        await db.InsertBatchAsync(_toCommit);
    }

    private async Task RemoveExisitingOfflineDonationsAsync(IUmbracoDatabase db) {
        var toRemove = _toCommit.Select(x => x.TransactionReference).ToArray();

        var sql = new Sql("DELETE FROM Contribution WHERE TransactionReference IN (@0)", toRemove);
        
        await db.DeleteAsync(sql);
    }
}