using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Hosting;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Features.DynamicListViews;

public class DynamicListViewApiController : ApiController {
    private readonly IContentService _contentService;
    private readonly IDataTypeService _dataTypeService;

    public DynamicListViewApiController(IContentService contentService, IDataTypeService dataTypeService) {
        _contentService = contentService;
        _dataTypeService = dataTypeService;
    }

    [HttpGet("{contentId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid contentId) {
        var content = _contentService.GetById(contentId);

        if (content == null || !ContentPathHelper.DynamicListViewsEnabled(content.Path)) {
            return Ok(new { enabled = false });
        }

        /*TODO*/
        var dataTypeName = $"List View - {content.ContentType.Alias}";
        var dataType = await _dataTypeService.GetAsync(dataTypeName);

        return Ok(new { enabled = dataType != null });
    }
}