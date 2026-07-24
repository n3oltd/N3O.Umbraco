using Perplex.ContentBlocks.PropertyEditor;
using Perplex.ContentBlocks.PropertyEditor.Value;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.Editors;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Blocks.Perplex;

// Workaround for a Perplex.ContentBlocks v4 defect: ContentBlocksValueEditor.FromEditor builds the inner
// ContentPropertyData without ContentKey/PropertyTypeKey, so editors that store a file against the content key
// (Image Cropper and File Upload) receive Guid.Empty and throw "Invalid content key" on save. This repeats
// Perplex's FromEditor but sets both keys, exactly as Umbraco's own block editor does
// (BlockValuePropertyValueEditorBase.MapBlockItemDataFromEditor).
//
// Reported upstream — remove this folder (and the Exclude<> in PerplexBlocksComposer) once the fix ships in a
// Perplex.ContentBlocks release we can upgrade to:
//   Issue: https://github.com/PerplexDigital/Perplex.ContentBlocks/issues/102
//   PR:    https://github.com/PerplexDigital/Perplex.ContentBlocks/pull/103
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

        ContentBlocksValueUtils.Iterate(model, block => FromEditorBlock(block.Content));

        return _jsonSerializer.Serialize(model);
    }

    private void FromEditorBlock(BlockItemData data) {
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

            var propData = new ContentPropertyData(prop.Value, configuration) {
                ContentKey = data.Key,
                PropertyTypeKey = prop.PropertyType.Key
            };

            prop.Value = valueEditor.FromEditor(propData, prop.Value);
        }
    }
}
