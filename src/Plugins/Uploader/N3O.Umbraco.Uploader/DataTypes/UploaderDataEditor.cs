using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Uploader.DataTypes;

[DataEditor(UploaderConstants.PropertyEditorAlias, ValueType = ValueTypes.Json)]
public class UploaderDataEditor : DataEditor {
    private readonly IIOHelper _ioHelper;

    public UploaderDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                              IIOHelper ioHelper)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new UploaderConfigurationEditor(_ioHelper);
    }
}
