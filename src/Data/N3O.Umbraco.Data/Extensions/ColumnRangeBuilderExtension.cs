using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Extensions {
    public static class ColumnRangeBuilderExtension {
        public static IReadOnlyList<Column> GetColumns(this IColumnRangeBuilder columnRangeBuilder,
                                                       ColumnTemplate columnTemplate) {
            var columnRange = columnRangeBuilder.String<string>()
                                                .Title(columnTemplate.Heading)
                                                .Build();

            columnRange.AddCells(0,
                                 columnTemplate.MaxValues == 1
                                     ? ""
                                     : Enumerable.Repeat("", columnTemplate.MaxValues));

            return columnRange.GetColumns().ToList();
        }

        public static IReadOnlyList<string> GetColumnHeadings(this IColumnRangeBuilder columnRangeBuilder,
                                                              ColumnTemplate columnTemplate) {
            var headings = GetColumns(columnRangeBuilder, columnTemplate)
                          .Select(x => x.Title)
                          .ToList();

            return headings;
        }
    }
}