using Perplex.ContentBlocks.PropertyEditor;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;

namespace N3O.Umbraco.Blocks.Perplex;

// Claims Perplex's editor alias, which the composer frees by excluding Perplex's own editor, so
// N3OContentBlocksValueEditor is the value editor in play.
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
