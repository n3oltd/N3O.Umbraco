// ReSharper disable Mvc.ViewNotResolved

using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Context;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Donations.Content;
using N3O.Umbraco.Giving.Donations.ViewModels;
using N3O.Umbraco.Giving.Pricing;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Localization;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Donations.Controllers;

[ResponseCache(CacheProfileName = CacheProfiles.NoCache)]
// TODO Fix the base class
public class DonationOptionsController : Controller {
    private readonly IDonationForms _donationForms;
    private readonly IForexConverter _forexConverter;
    private readonly ICurrencyAccessor _currencyAccessor;
    private readonly IPricing _pricing;
    private readonly IFormatter _formatter;
    private readonly IUmbracoMapper _mapper;

    public DonationOptionsController(IDonationForms donationForms,
                                     IForexConverter forexConverter,
                                     ICurrencyAccessor currencyAccessor,
                                     IPricing pricing,
                                     IFormatter formatter,
                                     IUmbracoMapper mapper) {
        _donationForms = donationForms;
        _forexConverter = forexConverter;
        _currencyAccessor = currencyAccessor;
        _pricing = pricing;
        _formatter = formatter;
        _mapper = mapper;
    }
    
    [HttpGet]
    public ActionResult Single(Guid id) {
        return Get(id, DonationTypes.Single);
    }
    
    [HttpGet]
    public ActionResult Regular(Guid id) {
        return Get(id, DonationTypes.Regular);
    }
    
    private ActionResult Get(Guid id, DonationType donationType) {
        var donationOption = _donationForms.GetOption(id);

        if (donationOption is FundDonationOption fundDonationOption) {
            return Fund(fundDonationOption, donationType);
        } else if (donationOption is SponsorshipDonationOption sponsorshipDonationOption) {
            return Sponsorship(sponsorshipDonationOption, donationType);   
        } else {
            return NotFound($"No donation option found with ID {id}");
        }
    }

    private ActionResult Fund(FundDonationOption fundOption, DonationType donationType) {
        var viewModel = new FundDonationFormViewModel(_forexConverter,
                                                      _pricing,
                                                      _mapper,
                                                      _currencyAccessor,
                                                      _formatter,
                                                      fundOption,
                                                      donationType);

        return View("~/Views/DonationForms/Fund.cshtml", viewModel);
    }
    
    private ActionResult Sponsorship(SponsorshipDonationOption sponsorship, DonationType donationType) {
        var viewModel = new SponsorshipDonationFormViewModel(_currencyAccessor,
                                                             _formatter,
                                                             _pricing,
                                                             _mapper,
                                                             sponsorship,
                                                             donationType);
        
        return View("~/Views/DonationForms/Sponsorship.cshtml", viewModel);
    }
}
