using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Controllers;

[ApiDocument(CrowdfundingConstants.ApiName)]
public class CrowdfundingPageController : ApiController {
    private readonly IFundraisingPages _fundraisingPages;

    public CrowdfundingPageController(IFundraisingPages fundraisingPages) {
        _fundraisingPages = fundraisingPages;
    }
    
    [HttpPost("page/{pageName}/create")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task CreateContent([FromRoute] string pageName) {
        _fundraisingPages.CreatePage(pageName);
        
        return Task.CompletedTask;
    }

    public class PropertyValueReq {
        public string Alias { get; set; }
        
        public Type Type { get; set; }
        public BoolValueReq Bool { get; set; }
        public IntValueReq Int { get; set; }
        public CropperValueReq Cropper { get; set; }
        
        // separate validators that kick on on each automatically
    }

    public class BoolValueReq {
        public bool? Value { get; set; }
    }

    public class IntValueReq {
        public bool? Value { get; }
    }

    public class CropperValueReq {
        //x. y, width and height proprties
    }

    /* The data type on the req model should provide the method to apply the update to the property so this is defined
     in one place and made as simple as possible. */
    
    [HttpPut("{pageId:guid}/{propertyAlias}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdatePropertyByAlias([FromRoute] Guid pageId, [FromRoute] string propertyAlias, PropertyValueReq value) {
        await _fundraisingPages.UpdatePropertyAsync(pageId,
                                                    null,
                                                    x => x.Content.Boolean(propertyAlias).Set((bool) value.Value));
    }
    

    /*[HttpPut("{pageId:guid}/{propertyAlias}/nested")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateNestedField(FieldData data) {
        await _fundraisingPages.UpdatePropertyAsync(Guid.Parse(data.PageId),
                                                    data.PageTitle, x => x.Content.Property<BooleanPropertyBuilder>(propertyAlias).Set(data.Data));
    }*/

    [HttpPut("{pageId:guid}/{propertyAlias}/numeric")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateNumericField([FromRoute] Guid pageId, [FromRoute] string propertyAlias, PropertyValue value) {
        await _fundraisingPages.UpdatePropertyAsync(pageId,
                                                    null,
                                                    x => x.Content.Numeric(propertyAlias).SetDecimal((decimal) value.Value));
    }

    [HttpPut("{pageId:guid}/{propertyAlias}/radioButtonList")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateRadioButtonListField([FromRoute] Guid pageId, [FromRoute] string propertyAlias, PropertyValue value) {
        await _fundraisingPages.UpdatePropertyAsync(pageId,
                                                    null,
                                                    x => x.Content.RadioButtonList(propertyAlias).Set(value.Value.ToString()));
    }

    [HttpPut("{pageId:guid}/{propertyAlias}/rawProperty")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateRawPropertyField([FromRoute] Guid pageId, [FromRoute] string propertyAlias, PropertyValue value) {
        await _fundraisingPages.UpdatePropertyAsync(pageId,
                                                    null,
                                                    x => x.Content.Raw(propertyAlias).Set(value.Value));
    }

    [HttpPut("{pageId:guid}/{propertyAlias}/templatedLabel")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateTemplatedLabelField([FromRoute] Guid pageId, [FromRoute] string propertyAlias, PropertyValue value) {
        await _fundraisingPages.UpdatePropertyAsync(pageId,
                                                    null,
                                                    x => x.Content.TemplatedLabel(propertyAlias).Set(value.Value));
    }

    [HttpPut("{pageId:guid}/{propertyAlias}/textarea")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateTextareaField([FromRoute] Guid pageId, [FromRoute] string propertyAlias, PropertyValue value) {
        await _fundraisingPages.UpdatePropertyAsync(pageId,
                                                    null,
                                                    x => x.Content.Textarea(propertyAlias).Set(value.Value.ToString()));
    }

    [HttpPut("{pageId:guid}/{propertyAlias}/textBox")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateTextBoxField([FromRoute] Guid pageId, [FromRoute] string propertyAlias, PropertyValue value) {
        await _fundraisingPages.UpdatePropertyAsync(pageId,
                                                    null,
                                                    x => x.Content.TextBox(propertyAlias).Set(value.Value.ToString()));
    }

    [HttpPut("{pageId:guid}/{propertyAlias}/toggle")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task UpdateToggleField([FromRoute] Guid pageId, [FromRoute] string propertyAlias, PropertyValue value) {
        await _fundraisingPages.UpdatePropertyAsync(pageId,
                                                    null,
                                                    x => x.Content.Toggle(propertyAlias).Set((bool) value.Value));
    }
}