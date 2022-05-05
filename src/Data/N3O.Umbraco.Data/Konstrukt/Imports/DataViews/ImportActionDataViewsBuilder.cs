using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Konstrukt.Configuration.Builders.DataViews;
using Konstrukt.Models;

namespace N3O.Umbraco.Data.Konstrukt {
    public class ImportActionDataViewsBuilder : KonstruktDataViewsBuilder<Import> {
        public override IEnumerable<KonstruktDataViewSummary> GetDataViews() {
            yield return new KonstruktDataViewSummary {
                Alias = "all",
                Name = "All",
                Group = "Action"
            };

            foreach (var status in ImportActions.All) {
                yield return new KonstruktDataViewSummary {
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
}