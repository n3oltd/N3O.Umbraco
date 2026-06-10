using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Hosting;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Features.DynamicListViews;

// Anonymous on purpose: returns only a boolean "is the dynamic list view enabled for this node",
// which is low-sensitivity, and it is called from the (already authenticated) backoffice. Using the
// non-authorized ApiController base keeps the JSON/validation filters but drops backoffice auth, so
// the backoffice condition can call it with a plain fetch (no token handling).
public class DynamicListViewApiController : ApiController {
    private readonly IContentService _contentService;
    private readonly IDataTypeService _dataTypeService;

    public DynamicListViewApiController(IContentService contentService, IDataTypeService dataTypeService) {
        _contentService = contentService;
        _dataTypeService = dataTypeService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id) {
        var content = _contentService.GetById(id);

        if (content == null || !ContentPathHelper.DynamicListViewsEnabled(content.Path)) {
            return Ok(new { enabled = false });
        }

        var dataTypeName = $"List View - {content.ContentType.Alias}";
        var dataType = await _dataTypeService.GetAsync(dataTypeName);

        return Ok(new { enabled = dataType != null });
    }
}