using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public abstract class PropertyType : Lookup {
    private readonly Action<MapperContext, IPublishedProperty, PagePropertyValueRes> _populateRes;

    protected PropertyType(string id,
                           Action<MapperContext, IPublishedProperty, PagePropertyValueRes> populateRes,
                           params string[] editorAliases) : base(id) {
        _populateRes = populateRes;
        EditorAliases = editorAliases;
    }

    public IEnumerable<string> EditorAliases { get; }

    public abstract Task UpdatePropertyAsync(IContentPublisher contentPublisher, string alias, object data);

    public void PopulateRes(MapperContext ctx, IPublishedProperty src, PagePropertyValueRes dest) {
        _populateRes(ctx, src, dest);
    }
}

public abstract class PropertyType<TReq> : PropertyType {
    protected PropertyType(string id,
                           Action<MapperContext, IPublishedProperty, PagePropertyValueRes> populateRes,
                           params string[] editorAliases)
        : base(id, populateRes, editorAliases) { }

    public override async Task UpdatePropertyAsync(IContentPublisher contentPublisher, string alias, object data) {
        await UpdatePropertyAsync(contentPublisher, alias, (TReq) data);
    }

    protected abstract Task UpdatePropertyAsync(IContentPublisher contentPublisher, string alias, TReq data);
}

public class PropertyTypes : StaticLookupsCollection<PropertyType> {
    public static readonly PropertyType Boolean = new BooleanPropertyType();
    public static readonly PropertyType Cropper = new CropperPropertyType();
    public static readonly PropertyType DateTime = new DateTimePropertyType();
    public static readonly PropertyType Numeric = new NumericPropertyType();
    public static readonly PropertyType Raw = new RawPropertyType();
    public static readonly PropertyType Textarea = new TextareaPropertyType();
    public static readonly PropertyType TextBox = new TextBoxPropertyType();
}