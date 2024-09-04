using Konstrukt.Configuration.Builders.DataViews;
using Konstrukt.Models;
using N3O.Umbraco.Crowdfunding.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace N3O.Umbraco.Crowdfunding.UIBuilder;

public class OnlineContributionStatusDataViewsBuilder : KonstruktDataViewsBuilder<OnlineContribution> {
    private const string AllAlias = "all";
    private const string Group = "Status";

    public override IEnumerable<KonstruktDataViewSummary> GetDataViews() {
        yield return new KonstruktDataViewSummary {
            Alias = AllAlias,
            Name = "All",
            Group = Group
        };
        
        foreach (var status in OnlineContributionStatuses.All) {
            yield return new KonstruktDataViewSummary {
                Alias = status,
                Name = status,
                Group = Group
            };
        }
    }

    public override Expression<Func<OnlineContribution, bool>> GetDataViewWhereClause(string dataViewAlias) {
        if (dataViewAlias == AllAlias) {
            return null;
        } else {
            return c => c.Status == dataViewAlias;
        }
    }
}
