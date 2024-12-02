using N3O.Umbraco.Bundling;
using Smidge;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingAssets : IAssetBundle {
    private readonly ICrowdfundingRouter _crowdfundingRouter;

    public CrowdfundingAssets(ICrowdfundingRouter crowdfundingRouter) {
        _crowdfundingRouter = crowdfundingRouter;
    }
    
    public void Require(ISmidgeRequire bundle) {
        bundle.RequiresCss("~/assets/css/crowdfunding-main.css");
        bundle.RequiresCss("~/assets/css/intlTelInput.css");
        bundle.RequiresCss("~/assets/css/banner.css");
        
        bundle.RequiresJs("~/assets/js/jquery.js");
        bundle.RequiresJs("~/assets/js/slick.min.js");
        bundle.RequiresJs("~/assets/js/crowdfunding-main.js");
        bundle.RequiresJs("~/assets/js/accounts.js");
        bundle.RequiresJs("~/assets/js/tabs.js");
        bundle.RequiresJs("~/assets/js/modals.js");
        bundle.RequiresJs("~/assets/js/numberVal.js");
        bundle.RequiresJs("~/assets/js/cta-box.js");
        
        _crowdfundingRouter.CurrentPage?.AddAssets(bundle);
    }
}