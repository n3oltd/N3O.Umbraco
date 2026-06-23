using Umbraco.UIBuilder.Configuration.Builders.DataViews;
using Umbraco.UIBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace N3O.Umbraco.Data.UIBuilder;

public class ImportActionDataViewsBuilder : DataViewsBuilder<Import> {
    public override IEnumerable<DataViewSummary> GetDataViews() {
        yield return new DataViewSummary {
            Alias = "all",
            Name = "All",
            Group = "Action"
        };

        foreach (var status in ImportActions.All) {
            yield return new DataViewSummary {
                Alias = status,
                Name = status,
                Group = "Action"
            };
        }
    }

    public override Expression<Func<Import, bool>> GetDataViewWhereClause(string dataViewAlias) {
        if (dataViewAlias == "all") {
            return null;
        }

        return c => c.Action == dataViewAlias;
    }
}
