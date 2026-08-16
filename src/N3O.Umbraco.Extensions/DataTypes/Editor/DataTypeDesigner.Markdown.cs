using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.DataTypes;

public class MarkdownDataTypeDesigner : DataTypeDesigner {
    private string _defaultValue;
    private bool _displayLivePreview;

    public MarkdownDataTypeDesigner(IDataTypeService dataTypeService,
                                    PropertyEditorCollection propertyEditors,
                                    IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public MarkdownDataTypeDesigner DefaultValue(string defaultValue) {
        _defaultValue = defaultValue;

        return this;
    }

    public MarkdownDataTypeDesigner DisplayLivePreview() {
        _displayLivePreview = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new MarkdownConfiguration();

        configuration.DisplayLivePreview = _displayLivePreview;

        if (_defaultValue.HasValue()) {
            configuration.DefaultValue = _defaultValue;
        }

        return configuration;
    }

    protected override string EditorAlias => UmbracoPropertyEditors.Aliases.MarkdownEditor;
}
