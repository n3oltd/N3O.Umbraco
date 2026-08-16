using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;
using UmbracoUdiEntityType = Umbraco.Cms.Core.Constants.UdiEntityType;

namespace N3O.Umbraco.DataTypes;

public class RichTextDataTypeDesigner : DataTypeDesigner {
    private readonly IContentTypeService _contentTypeService;
    private readonly List<string> _blockElementTypeAliases = [];

    private object _editor;
    private bool _hideLabel;
    private bool _ignoreUserStartNodes;
    private Guid? _mediaParentKey;
    private string _overlaySize;
    private bool _useLiveEditing;

    public RichTextDataTypeDesigner(IDataTypeService dataTypeService,
                                    IContentTypeService contentTypeService,
                                    PropertyEditorCollection propertyEditors,
                                    IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) {
        _contentTypeService = contentTypeService;
    }

    public RichTextDataTypeDesigner AllowBlocks(params string[] elementTypeAliases) {
        _blockElementTypeAliases.AddRange(elementTypeAliases);

        return this;
    }

    public RichTextDataTypeDesigner Editor(object editor) {
        _editor = editor;

        return this;
    }

    public RichTextDataTypeDesigner HideLabel() {
        _hideLabel = true;

        return this;
    }

    public RichTextDataTypeDesigner IgnoreUserStartNodes() {
        _ignoreUserStartNodes = true;

        return this;
    }

    public RichTextDataTypeDesigner MediaParent(Guid mediaFolderKey) {
        _mediaParentKey = mediaFolderKey;

        return this;
    }

    public RichTextDataTypeDesigner OverlaySize(string overlaySize) {
        _overlaySize = overlaySize;

        return this;
    }

    public RichTextDataTypeDesigner UseLiveEditing() {
        _useLiveEditing = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new RichTextConfiguration();

        configuration.Editor = _editor;
        configuration.HideLabel = _hideLabel;
        configuration.IgnoreUserStartNodes = _ignoreUserStartNodes;
        configuration.OverlaySize = _overlaySize;
        configuration.UseLiveEditing = _useLiveEditing;

        if (_mediaParentKey.HasValue()) {
            configuration.MediaParentId = new GuidUdi(UmbracoUdiEntityType.Media,
                                                      _mediaParentKey.GetValueOrThrow());
        }

        if (_blockElementTypeAliases.Count > 0) {
            configuration.Blocks = _blockElementTypeAliases.Select(BuildBlock).ToArray();
        }

        return configuration;
    }

    protected override string EditorAlias => UmbracoPropertyEditors.Aliases.TinyMce;

    private RichTextConfiguration.RichTextBlockConfiguration BuildBlock(string elementTypeAlias) {
        var elementType = _contentTypeService.Get(elementTypeAlias);

        if (elementType == null) {
            throw new Exception($"No element type found with alias {elementTypeAlias.Quote()}");
        }

        var block = new RichTextConfiguration.RichTextBlockConfiguration();

        block.ContentElementTypeKey = elementType.Key;

        return block;
    }
}
