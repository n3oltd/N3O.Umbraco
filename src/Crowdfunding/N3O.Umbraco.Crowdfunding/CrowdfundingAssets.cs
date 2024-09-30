using N3O.Umbraco.Bundling;
using Smidge;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingAssets : IAssetBundle {
    public void Require(ISmidgeRequire bundle) {
        bundle.RequiresCss("~/assets/css/crowdfunding-main.css");
        bundle.RequiresCss("~/assets/css/intlTelInput.css");
        
        bundle.RequiresJs("~/assets/js/jquery.js");
        bundle.RequiresJs("~/assets/js/slick.min.js");
        bundle.RequiresJs("~/assets/js/crowdfunding-main.js");
        bundle.RequiresJs("~/assets/js/accounts.js");
        bundle.RequiresJs("~/assets/js/tabs.js");
        bundle.RequiresJs("~/assets/js/modals.js");
        bundle.RequiresJs("~/assets/js/numberVal.js");
        bundle.RequiresJs("~/assets/js/cta-box.js");
    }
}