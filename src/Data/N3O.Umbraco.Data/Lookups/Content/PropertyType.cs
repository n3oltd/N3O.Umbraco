using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Lookups;

public abstract class PropertyType : Lookup {
    private readonly Action<MapperContext, PublishedContentProperty, ContentPropertyValueRes> _populateValueRes;
    private readonly Func<MapperContext, ContentPropertyConfiguration, ContentPropertyConfigurationRes> _getConfigurationRes;

    protected PropertyType(string id,
                           Action<MapperContext, PublishedContentProperty, ContentPropertyValueRes> populateValueRes,
                           Func<MapperContext, ContentPropertyConfiguration, ContentPropertyConfigurationRes> getConfigurationRes,
                           params string[] editorAliases)
        : base(id) {
        _populateValueRes = populateValueRes;
        _getConfigurationRes = getConfigurationRes;
        DataTypeAttributeValue = id;
        EditorAliases = editorAliases;
    }

    public string DataTypeAttributeValue { get; }
    public IEnumerable<string> EditorAliases { get; }

    public abstract Task UpdatePropertyAsync(IContentBuilder contentBuilder, string alias, object data);
    
    public ContentPropertyConfigurationRes GetConfigurationRes(MapperContext ctx, string contentTypeAlias, string propertyAlias) {
        var src = new ContentPropertyConfiguration(contentTypeAlias, propertyAlias);
        
        return _getConfigurationRes(ctx, src);
    }

    public void PopulateValueRes(MapperContext ctx, PublishedContentProperty src, ContentPropertyValueRes dest) {
        _populateValueRes(ctx, src, dest);
    }
}

public abstract class PropertyType<TReq> : PropertyType {
    protected PropertyType(string id,
                           Action<MapperContext, PublishedContentProperty, ContentPropertyValueRes> populateValueRes,
                           Func<MapperContext, ContentPropertyConfiguration, ContentPropertyConfigurationRes> getConfigurationRes,
                           params string[] editorAliases)
        : base(id, populateValueRes, getConfigurationRes, editorAliases) { }

    public override async Task UpdatePropertyAsync(IContentBuilder contentBuilder, string alias, object data) {
        await UpdatePropertyAsync(contentBuilder, alias, (TReq) data);
    }

    protected abstract Task UpdatePropertyAsync(IContentBuilder contentBuilder, string alias, TReq data);
}

public class PropertyTypes : StaticLookupsCollection<PropertyType> {
    public static readonly PropertyType Boolean = new BooleanPropertyType();
    public static readonly PropertyType Cropper = new CropperPropertyType();
    public static readonly PropertyType DateTime = new DateTimePropertyType();
    public static readonly PropertyType Nested = new NestedPropertyType();
    public static readonly PropertyType Numeric = new NumericPropertyType();
    public static readonly PropertyType Raw = new RawPropertyType();
    public static readonly PropertyType Textarea = new TextareaPropertyType();
    public static readonly PropertyType TextBox = new TextBoxPropertyType();
}