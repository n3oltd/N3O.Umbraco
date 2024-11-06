using N3O.Umbraco.Extensions;
using NPoco;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class SqlExtensions {
    public static Sql SelectTop(this Sql sql, string columns, int? top = null) {
        if (top.HasValue()) {
            return sql.Append($"SELECT TOP {top} {columns}");
        } else {
            return sql.Append($"SELECT {columns}");
        }
    }
}