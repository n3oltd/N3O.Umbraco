using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.DataTypes;

public class RadioButtonListDataTypeDesigner : ValueListDataTypeDesigner<RadioButtonListDataTypeDesigner> {
    public RadioButtonListDataTypeDesigner(IDataTypeService dataTypeService,
                                           PropertyEditorCollection propertyEditors,
                                           IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    protected override string EditorAlias =>
        UmbracoPropertyEditors.Aliases.RadioButtonList;
}
