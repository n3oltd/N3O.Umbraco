using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Content;
using N3O.Umbraco.Plugins.Controllers;
using N3O.Umbraco.SerpEditor.Content;
using N3O.Umbraco.SerpEditor.Models;

namespace N3O.Umbraco.SerpEditor.Controllers;

public class SerpEditorController : PluginController {
    private readonly IContentCache _contentCache;

    public SerpEditorController(IContentCache contentCache) {
        _contentCache = contentCache;
    }
    
    [HttpGet("templateOptions")]
    public ActionResult<TemplateOptionsRes> GetTemplateOptions() {
        var templateOptions = _contentCache.Single<TemplateContent>();

        var res = new TemplateOptionsRes();
        res.TitleSuffix = templateOptions?.TitleSuffix;

        return Ok(res);
    }
}
