using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.TextResourceEditor.DataTypes;

[DataEditor(TextResourceEditorConstants.PropertyEditorAlias, ValueType = ValueTypes.Json)]
public class TextResourceEditorDataEditor : DataEditor {
    private readonly IIOHelper _ioHelper;

    public TextResourceEditorDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                                        IIOHelper ioHelper)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new TextResourceEditorConfigurationEditor(_ioHelper);
    }
}
