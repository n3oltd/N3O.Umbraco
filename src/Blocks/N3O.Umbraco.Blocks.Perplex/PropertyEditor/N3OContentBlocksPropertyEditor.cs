using Perplex.ContentBlocks.PropertyEditor;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;

namespace N3O.Umbraco.Blocks.Perplex;

// Registers under the same alias as Perplex's own editor (which PerplexBlocksComposer excludes) so the framework
// resolves N3OContentBlocksValueEditor instead of the defective Perplex one. See N3OContentBlocksValueEditor.
[DataEditor("Perplex.ContentBlocks", ValueEditorIsReusable = false, ValueType = ValueTypes.Json)]
public class N3OContentBlocksPropertyEditor : PerplexContentBlocksPropertyEditor {
    public N3OContentBlocksPropertyEditor(IDataValueEditorFactory dataValueEditorFactory,
                                          IIOHelper ioHelper,
                                          IConfigurationEditorJsonSerializer configEditorSerializer,
                                          ContentBlocksPropertyIndexValueFactory propertyIndexValueFactory)
        : base(dataValueEditorFactory, ioHelper, configEditorSerializer, propertyIndexValueFactory) { }

    protected override IDataValueEditor CreateValueEditor() {
        return DataValueEditorFactory.Create<N3OContentBlocksValueEditor>(Attribute);
    }
}
