using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cropper.DataTypes;

[DataEditor(CropperConstants.PropertyEditorAlias,
            "N3O Cropper",
            "~/App_Plugins/N3O.Umbraco.Cropper/N3O.Umbraco.Cropper.html",
            HideLabel = false,
            ValueType = ValueTypes.Json)]
public class CropperDataEditor : DataEditor {
    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    public CropperDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                             IIOHelper ioHelper,
                             IEditorConfigurationParser editorConfigurationParser)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new CropperConfigurationEditor(_ioHelper, _editorConfigurationParser);
    }
}
