using N3O.Umbraco.Data.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Models; 

public class ContentTypeMapping : IMapDefinition {
    private readonly IDataTypeService _dataTypeService;
    private readonly IContentTypeService _contentTypeService;

    public ContentTypeMapping(IDataTypeService dataTypeService, IContentTypeService contentTypeService) {
        _dataTypeService = dataTypeService;
        _contentTypeService = contentTypeService;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IContentType, ContentTypeRes>((_, _) => new ContentTypeRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IContentType src, ContentTypeRes dest, MapperContext ctx) {
        dest.Alias = src.Alias;
        dest.Name = src.Name;
        dest.Properties = src.GetUmbracoProperties(_dataTypeService, _contentTypeService)
                             .Select(ToUmbracoPropertyInfoRes)
                             .ToList();
    }
    
    private UmbracoPropertyInfoRes ToUmbracoPropertyInfoRes(UmbracoPropertyInfo src) {
        var res = new UmbracoPropertyInfoRes();
        
        res.Alias = src.Type.Alias;
        res.Group = src.Group.Name;
        res.DataType = src.DataType.EditorAlias;
        res.Name = src.Type.Name;

        return res;
    }
}