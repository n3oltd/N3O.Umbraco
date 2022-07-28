using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Security;
using N3O.Umbraco.SerpEditor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.SerpEditor.Data.Converters;

public class SerpEditorPropertyConverter : PropertyConverter<string> {
    private readonly IColumnRangeBuilder _columnRangeBuilder;
    private readonly IContentHelper _contentHelper;

    public SerpEditorPropertyConverter(IColumnRangeBuilder columnRangeBuilder,
                                       IContentHelper contentHelper,
                                       ILocalizationSettingsAccessor localizationSettingsAccessor,
                                       ILocalClock localClock,
                                       IFormatter formatter)
        : base(columnRangeBuilder) {
        _columnRangeBuilder = columnRangeBuilder;
        _contentHelper = contentHelper;
    }

    public override bool IsConverter(UmbracoPropertyInfo propertyInfo) {
        return propertyInfo.Type.PropertyEditorAlias.EqualsInvariant(SerpEditorConstants.PropertyEditorAlias);
    }

    protected override IEnumerable<Cell<string>> GetCells(IContentProperty contentProperty,
                                                          UmbracoPropertyInfo propertyInfo) {
        var serpEntry = _contentHelper.GetSerpEntry(contentProperty);

        yield return OurDataTypes.String.Cell(serpEntry.IfNotNull(x => x.Title));
        yield return OurDataTypes.String.Cell(serpEntry.IfNotNull(x => x.Description));
    }

    public override void Import(IContentBuilder contentBuilder,
                                IEnumerable<IPropertyConverter> converters,
                                IParser parser,
                                ErrorLog errorLog,
                                string columnTitlePrefix,
                                UmbracoPropertyInfo propertyInfo,
                                IEnumerable<ImportField> fields) {
        Import(errorLog,
               propertyInfo,
               fields,
               s => parser.String.Parse(s, OurDataTypes.String.GetClrType()),
               (alias, value) => value.IfNotNull(x => contentBuilder.SerpEditor(alias).SetTitle(value)).Yield());
    }

    
    /*public override IReadOnlyList<Column> GetColumns(IEnumerable<IPropertyConverter> converters,
                                                     UmbracoPropertyInfo propertyInfo,
                                                     string columnTitlePrefix) {
        propertyInfo.GetColumnTitle(columnTitlePrefix);
        var titleTemplate = new ColumnTemplate(GetColumnTitlePrefix(propertyInfo, columnTitlePrefix, "Title"),
                                               1,
                                               propertyInfo);
        
        var descriptionTemplate = new ColumnTemplate(GetColumnTitlePrefix(propertyInfo, columnTitlePrefix, "Description"),
                                               1,
                                               propertyInfo);

        var columns = _columnRangeBuilder.GetColumns(descriptionTemplate).ToList();
            columns.AddRange(_columnRangeBuilder.GetColumns(titleTemplate));
        
        return columns.AsReadOnly();
    }

    private string GetColumnTitlePrefix(UmbracoPropertyInfo propertyInfo,
                                        string columnTitlePrefix,
                                        string propertyName) {
        var titlePrefix = propertyInfo.GetColumnTitle(columnTitlePrefix);

        titlePrefix += $" {propertyInfo.ContentType.Name}: ";
        titlePrefix += $" {propertyName}";

        return titlePrefix;
    }*/
}