using N3O.Umbraco.Extensions;
using NodaTime;
using System.Text;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class LocalDateRangeExtensions {
    public static string FilterColumn(this Range<LocalDate?> range, string columnName) {
        var sb = new StringBuilder();

        if (range.From.HasValue() || range.To.HasValue()) {
            sb.Append("(");
            
            if (range.From.HasValue()) {
                sb.Append($"{columnName} >= '{range.From.GetValueOrThrow().ToYearMonthDayString()}'");
            }

            if (range.To.HasValue()) {
                sb.Append($"AND {columnName} <= '{range.To.GetValueOrThrow().ToYearMonthDayString()}'");
            }
            
            sb.Append(")");
        }

        return sb.ToString();
    }
}