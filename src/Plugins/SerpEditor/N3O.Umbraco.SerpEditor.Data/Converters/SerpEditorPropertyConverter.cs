using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.SerpEditor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.SerpEditor.Data.Converters;

public class SerpEditorPropertyConverter : IPropertyConverter {
    private readonly IColumnRangeBuilder _columnRangeBuilder;
    private readonly IContentHelper _contentHelper;
    private readonly IFormatter _formatter;
    private readonly Dictionary<string, IColumnRange> _columnRanges = new(StringComparer.InvariantCultureIgnoreCase);

    public SerpEditorPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                       IContentHelper contentHelper,
                                       IFormatter formatter) {
        _columnRangeBuilder = columnRangeBuilder;
        _contentHelper = contentHelper;
        _formatter = formatter;
    }

    public bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(SerpEditorConstants.PropertyEditorAlias);
    }
    
    public IReadOnlyList<Column> GetColumns(IEnumerable<IPropertyConverter> converters,
                                            UmbracoPropertyInfo propertyInfo,
                                            string columnTitlePrefix) {
        var columns = new List<Column>();

        columns.AddRange(_columnRangeBuilder.GetColumns(GetColumnTemplate(propertyInfo, columnTitlePrefix, x => x.Title)));
        columns.AddRange(_columnRangeBuilder.GetColumns(GetColumnTemplate(propertyInfo, columnTitlePrefix, x => x.Description)));

        return columns;
    }

    public void Export(IUntypedTableBuilder tableBuilder,
                       IEnumerable<IPropertyConverter> converters,
                       int columnOrder,
                       string columnTitlePrefix,
                       IContentProperty contentProperty,
                       UmbracoPropertyInfo propertyInfo) {
        var serpEntry = _contentHelper.GetSerpEntry(contentProperty);

        tableBuilder.AddCell(GetOrAddColumnRange(propertyInfo, columnTitlePrefix, columnOrder, x => x.Title),
                             OurDataTypes.String.Cell(serpEntry.IfNotNull(x => x.Title)));
        
        tableBuilder.AddCell(GetOrAddColumnRange(propertyInfo, columnTitlePrefix, columnOrder, x => x.Description),
                             OurDataTypes.String.Cell(serpEntry.IfNotNull(x => x.Description)));
    }

    public void Import(IContentBuilder contentBuilder,
                       IEnumerable<IPropertyConverter> converters,
                       IParser parser,
                       ErrorLog errorLog,
                       string columnTitlePrefix,
                       UmbracoPropertyInfo propertyInfo,
                       IEnumerable<ImportField> fields) {
        var title = fields.FirstOrDefault()
                          .IfNotNull(x => parser.String
                                                .Parse(x.Value, OurDataTypes.String.GetClrType())
                                                .Value);

        var description = fields.ElementAtOrDefault(1)
                                .IfNotNull(x => parser.String
                                                      .Parse(x.Value, OurDataTypes.String.GetClrType())
                                                      .Value);
        
        contentBuilder.SerpEditor(propertyInfo.Type.Alias).SetTitle(title).SetDescription(description);
    }
    
    private IColumnRange GetOrAddColumnRange(UmbracoPropertyInfo propertyInfo,
                                             string columnTitlePrefix,
                                             int columnOrder,
                                             Func<Strings, string> propertySelector) {
        var columnTitle = GetColumnTitle(propertyInfo, columnTitlePrefix, propertySelector);
        
        return _columnRanges.GetOrAdd(columnTitle,
                                      () => _columnRangeBuilder.OfType<string>(OurDataTypes.String)
                                                               .Title(columnTitle)
                                                               .SetOrder(columnOrder)
                                                               .PreserveColumnOrder()
                                                               .Build());
    }
    
    private ColumnTemplate GetColumnTemplate(UmbracoPropertyInfo propertyInfo,
                                             string columnTitlePrefix,
                                             Func<Strings, string> propertySelector) {
        var columnTitle = GetColumnTitle(propertyInfo, columnTitlePrefix, propertySelector);

        return new ColumnTemplate(columnTitle, 1, propertyInfo);
    }

    private string GetColumnTitle(UmbracoPropertyInfo propertyInfo,
                                  string columnTitlePrefix,
                                  Func<Strings, string> propertySelector) {
        return $"{propertyInfo.GetColumnTitle(columnTitlePrefix)} {_formatter.Text.Format(propertySelector)}";
    }

    public class Strings : CodeStrings {
        public string Description => "Description";
        public string Title => "Title";
    }
}