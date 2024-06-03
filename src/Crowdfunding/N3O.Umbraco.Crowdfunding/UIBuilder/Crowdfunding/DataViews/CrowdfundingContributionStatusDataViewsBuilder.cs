using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Konstrukt.Configuration.Builders.DataViews;
using Konstrukt.Models;

namespace N3O.Umbraco.Crowdfunding.UIBuilder;

public class CrowdfundingContributionStatusDataViewsBuilder : KonstruktDataViewsBuilder<CrowdfundingContribution> {
    private const string AllAlias = "all";
    private const string Group = "Status";

    public override IEnumerable<KonstruktDataViewSummary> GetDataViews() {
        yield return new KonstruktDataViewSummary {
            Alias = AllAlias,
            Name = "All",
            Group = Group
        };
        
        foreach (var status in CrowdfundingContributionStatuses.All) {
            yield return new KonstruktDataViewSummary {
                Alias = status,
                Name = status,
                Group = Group
            };
        }
    }

    public override Expression<Func<CrowdfundingContribution, bool>> GetDataViewWhereClause(string dataViewAlias) {
        if (dataViewAlias == AllAlias) {
            return null;
        } else {
            return c => c.Status == dataViewAlias;
        }
    }
}
