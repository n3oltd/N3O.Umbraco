using Microsoft.AspNetCore.Builder;
using N3O.Umbraco.Giving.Donations.Controllers;
using N3O.Umbraco.Hosting;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Giving.Donations; 

public class DonationEndpointsExtension : IUmbracoEndpointExtension {
    public void Run(IUmbracoEndpointBuilderContext u) {
        u.EndpointRouteBuilder.MapControllerRoute("donation-form-single-route",
                                                  "/donation-forms/options/single/{id:guid}",
                                                  new {
                                                      Controller = nameof(DonationOptionsController),
                                                      Action = nameof(DonationOptionsController.Single)
                                                  });
        
        u.EndpointRouteBuilder.MapControllerRoute("donation-form-regular-route",
                                                  "/donation-forms/options/regular/{id:guid}",
                                                  new {
                                                      Controller = nameof(DonationOptionsController),
                                                      Action = nameof(DonationOptionsController.Regular)
                                                  });
    }
}