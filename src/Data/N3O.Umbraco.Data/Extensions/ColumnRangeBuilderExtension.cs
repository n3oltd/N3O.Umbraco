using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Extensions {
    public static class ColumnRangeBuilderExtension {
        public static IEnumerable<Column> GetColumns(this IColumnRangeBuilder columnRangeBuilder, TemplateColumn templateColumn) {
            var columnRange = columnRangeBuilder.String<string>()
                                                .Title(templateColumn.Heading)
                                                .Build();

            columnRange.AddCells(0,
                                 templateColumn.MaxValues == 1
                                     ? ""
                                     : Enumerable.Repeat("", templateColumn.MaxValues));

            return columnRange.GetColumns().ToList();
        }

        public static IEnumerable<string> GetColumnHeadings(this IColumnRangeBuilder columnRangeBuilder, TemplateColumn templateColumn) {
            var headings = GetColumns(columnRangeBuilder, templateColumn)
                          .Select(x => x.Title)
                          .ToList();

            return headings;
        }
    }
}