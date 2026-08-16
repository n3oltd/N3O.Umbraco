using Humanizer;
using N3O.Umbraco.DataTypes;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public abstract class ConfiguredPropertyTypeBuilder<TSelf, TDesigner> : PropertyTypeBuilder<TSelf>
    where TSelf : ConfiguredPropertyTypeBuilder<TSelf, TDesigner>
    where TDesigner : IDataTypeDesigner {
    private readonly TDesigner _dataTypeDesigner;

    private Action<TDesigner> _configure;

    protected ConfiguredPropertyTypeBuilder(IDataTypeService dataTypeService, TDesigner dataTypeDesigner)
        : base(dataTypeService) {
        _dataTypeDesigner = dataTypeDesigner;
    }

    public TSelf Configure(Action<TDesigner> configure) {
        _configure = configure;

        return (TSelf) this;
    }

    protected IDataType BuildInlineDataType(PropertyTypeContext context) {
        _dataTypeDesigner.SetName($"{context.ContentTypeAlias.Titleize()} {context.PropertyAlias.Titleize()}");
        _dataTypeDesigner.WithoutNameAdoption();

        if (context.UseDeterministicIds) {
            _dataTypeDesigner.WithDeterministicId($"{context.ContentTypeAlias}_{context.PropertyAlias}");
        }

        ConfigureDataType(_dataTypeDesigner);

        _configure?.Invoke(_dataTypeDesigner);

        return _dataTypeDesigner.Save();
    }

    protected virtual void ConfigureDataType(TDesigner dataTypeDesigner) { }

    protected bool HasConfiguration => _configure != null;
}
