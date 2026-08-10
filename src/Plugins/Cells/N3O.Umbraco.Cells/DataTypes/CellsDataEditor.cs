using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Cells.DataTypes;

[DataEditor(CellsConstants.PropertyEditorAlias, ValueType = ValueTypes.Json)]
public class CellsDataEditor : DataEditor {
    private readonly IIOHelper _ioHelper;

    public CellsDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                           IIOHelper ioHelper)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new CellsConfigurationEditor(_ioHelper);
    }
}
