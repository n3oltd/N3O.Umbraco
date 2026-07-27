using Perplex.ContentBlocks.PropertyEditor;
using Perplex.ContentBlocks.PropertyEditor.Value;
using System;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.Editors;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Blocks.Perplex;

// Perplex's ContentBlocksValueEditor.FromEditor builds each inner ContentPropertyData without a ContentKey or
// PropertyTypeKey, so property editors that store a file against them throw "Invalid content key" on save.
// This repeats that method with both keys threaded down.
//
// TODO Remove this folder, the Exclude<> in PerplexBlocksComposer and the Perplex.ContentBlocks version
// pin once https://github.com/PerplexDigital/Perplex.ContentBlocks/issues/102 ships
public class N3OContentBlocksValueEditor : ContentBlocksValueEditor {
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ContentBlocksValueDeserializer _deserializer;
    private readonly PropertyEditorCollection _propertyEditors;
    private readonly IDataTypeConfigurationCache _dataTypeConfigCache;

    public N3OContentBlocksValueEditor(IShortStringHelper shortStringHelper,
                                       IJsonSerializer jsonSerializer,
                                       IIOHelper ioHelper,
                                       DataEditorAttribute attribute,
                                       ContentBlocksValidator validator,
                                       ContentBlocksValueDeserializer deserializer,
                                       PropertyEditorCollection propertyEditors,
                                       IDataTypeConfigurationCache dataTypeConfigCache,
                                       DataValueReferenceFactoryCollection referenceFactories)
        : base(shortStringHelper, jsonSerializer, ioHelper, attribute, validator, deserializer, propertyEditors,
               dataTypeConfigCache, referenceFactories) {
        _jsonSerializer = jsonSerializer;
        _deserializer = deserializer;
        _propertyEditors = propertyEditors;
        _dataTypeConfigCache = dataTypeConfigCache;
    }

    public override object FromEditor(ContentPropertyData editorValue, object currentValue) {
        if (_deserializer.Deserialize(editorValue.Value?.ToString()) is not ContentBlocksValue model) {
            return base.FromEditor(editorValue, currentValue);
        }

        ContentBlocksValueUtils.Iterate(model, block => FromEditorBlock(block.Content, editorValue.ContentKey));

        return _jsonSerializer.Serialize(model);
    }

    private void FromEditorBlock(BlockItemData data, Guid contentKey) {
        if (data == null) {
            return;
        }

        foreach (var prop in data.Values) {
            if (prop.PropertyType == null) {
                continue;
            }

            var configuration = _dataTypeConfigCache.GetConfiguration(prop.PropertyType.DataTypeKey);

            var propEditor = _propertyEditors[prop.PropertyType.PropertyEditorAlias];

            if (propEditor?.GetValueEditor(configuration) is not IDataValueEditor valueEditor) {
                continue;
            }

            var propData = new ContentPropertyData(prop.Value, configuration);
            propData.ContentKey = contentKey;
            propData.PropertyTypeKey = prop.PropertyType.Key;

            prop.Value = valueEditor.FromEditor(propData, prop.Value);
        }
    }
}
