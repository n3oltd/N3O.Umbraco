using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers; 

public class CrowdfundingController : ApiController {
    private readonly IFundraisingPages _fundraisingPages;

    public CrowdfundingController(IFundraisingPages fundraisingPages) {
        _fundraisingPages = fundraisingPages;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/boolean")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    /*[HttpPost("{pageId:guid}/{propertyId:int}/contentPicker")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/dataList")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/dateTime")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/dropdown")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/email")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/label")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/multipleText")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/nested")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/numeric")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/radioButtonList")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/textarea")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/textBox")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }
    
    [HttpPost("{pageId:guid}/{propertyId:int}/toggle")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindChildren([FromRoute] Guid pageId, [FromRoute] int propertyId) {
        await _fundraisingPages.UpdatePropertyAsync(pageId, x => x.Content.Property<BooleanPropertyBuilder>("").Set(true));

        return null;
    }*/
}