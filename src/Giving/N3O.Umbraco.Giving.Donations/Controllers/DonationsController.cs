using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Giving.Donations.Models;
using N3O.Umbraco.Hosting;
using System;

namespace N3O.Umbraco.Giving.Donations.Controllers {
    [ResponseCache(CacheProfileName = CacheProfiles.NoCache)]
    [ApiDocument(DonationConstants.ApiName)]
    public class DonationsController : ApiController {
        [HttpGet("forms/{id}")]
        public ActionResult<DonationFormRes> GetForm(Guid id) {
            throw new NotImplementedException();
        }
    }
}
