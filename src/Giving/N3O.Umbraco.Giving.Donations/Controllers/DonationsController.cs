using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Donations.Content;
using N3O.Umbraco.Giving.Donations.Models;
using N3O.Umbraco.Hosting;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Donations.Controllers {
    [ApiDocument(DonationConstants.ApiName)]
    public class DonationsController : ApiController {
        private readonly IContentLocator _contentLocator;
        private readonly IUmbracoMapper _umbracoMapper;

        public DonationsController(IContentLocator contentLocator, IUmbracoMapper umbracoMapper) {
            _contentLocator = contentLocator;
            _umbracoMapper = umbracoMapper;
        }
        
        [HttpGet("forms/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DonationFormRes> GetForm(Guid id) {
            var form = _contentLocator.ById<DonationFormContent>(id);

            if (form == null) {
                return NotFound();
            }

            var res = _umbracoMapper.Map<DonationFormContent, DonationFormRes>(form);

            return Ok(res);
        }
    }
}
