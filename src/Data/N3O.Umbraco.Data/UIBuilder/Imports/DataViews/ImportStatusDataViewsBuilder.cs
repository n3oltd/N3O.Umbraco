using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Konstrukt.Configuration.Builders.DataViews;
using Konstrukt.Models;

namespace N3O.Umbraco.Data.UIBuilder;

public class ImportStatusDataViewsBuilder : KonstruktDataViewsBuilder<Import> {
    private static readonly string AllAlias = "all";
    private static readonly string PendingAlias = "pending";
    private static readonly string Group = "Status";

    public override IEnumerable<KonstruktDataViewSummary> GetDataViews() {
        yield return new KonstruktDataViewSummary {
            Alias = PendingAlias,
            Name = "Pending (Queued or Error)",
            Group = Group
        };
        
        yield return new KonstruktDataViewSummary {
            Alias = AllAlias,
            Name = "All",
            Group = Group
        };
        
        foreach (var status in ImportStatuses.All) {
            yield return new KonstruktDataViewSummary {
                Alias = status,
                Name = status,
                Group = Group
            };
        }
    }

    public override Expression<Func<Import, bool>> GetDataViewWhereClause(string dataViewAlias) {
        if (dataViewAlias == PendingAlias) {
            return c => c.Status == ImportStatuses.Error || c.Status == ImportStatuses.Queued;
        } else if (dataViewAlias == AllAlias) {
            return null;
        } else {
            return c => c.Status == dataViewAlias;
        }
    }
}
