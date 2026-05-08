using Microsoft.AspNetCore.Mvc;
using System;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Features.DynamicListViews;

public class DynamicListViewApiController : N3O.Umbraco.Hosting.BackofficeAuthorizedApiController {
    private readonly IContentService _contentService;
    private readonly IDataTypeService _dataTypeService;

    public DynamicListViewApiController(IContentService contentService, IDataTypeService dataTypeService) {
        _contentService = contentService;
        _dataTypeService = dataTypeService;
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id) {
        var content = _contentService.GetById(id);

        if (content == null || !ContentPathHelper.DynamicListViewsEnabled(content.Path)) {
            return Ok(new { enabled = false });
        }

        var dataTypeName = $"List View - {content.ContentType.Alias}";
        var dataType = _dataTypeService.GetDataType(dataTypeName);

        return Ok(new { enabled = dataType != null });
    }
}