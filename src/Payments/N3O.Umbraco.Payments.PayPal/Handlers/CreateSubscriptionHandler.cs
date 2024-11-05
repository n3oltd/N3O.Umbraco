using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Checkout;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.PayPal.Clients;
using N3O.Umbraco.Payments.PayPal.Clients.Models;
using N3O.Umbraco.Payments.PayPal.Clients.PayPalErrors;
using N3O.Umbraco.Payments.PayPal.Commands;
using N3O.Umbraco.Payments.PayPal.Content;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCredential;
using Newtonsoft.Json;
using NUglify.Helpers;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Handlers;

public class CreateSubscriptionHandler : IRequestHandler<CreateSubscriptionCommand, None, PayPalCredential> {
    private readonly IPlansClient _plansClient;
    private readonly IProductsClient _productsClient;
    private readonly ISubscriptionsClient _subscriptionsClient;
    private readonly ICheckoutAccessor _checkoutAccessor;
    private readonly IContentCache _contentCache;

    public CreateSubscriptionHandler(ICheckoutAccessor checkoutAccessor,
                                     IContentCache contentCache,
                                     IPlansClient plansClient,
                                     IProductsClient productsClient,
                                     ISubscriptionsClient subscriptionsClient) {
        _checkoutAccessor = checkoutAccessor;
        _contentCache = contentCache;
        _plansClient = plansClient;
        _productsClient = productsClient;
        _subscriptionsClient = subscriptionsClient;
    }
    
    public async Task<PayPalCredential> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken) {
        //use refit to create a product - Use the constant value to retrieve the product with ID else create it 
        //ID hardcoded somewhere as it would be something like GER-Donations
        var settings = _contentCache.Single<PayPalSettingsContent>();

        var product = GetOrCreateProduct().Result;
        
        var plan = GetOrCreatePlan(product).Result;
        
        //use refit to create a plan - Use the Amount and currency as a ID (ID should be unique)
        //First use the get endpoint to retrieve, if not existing then create the plan
        var checkout = await _checkoutAccessor.GetAsync(cancellationToken);
        var totalAmount = checkout.RegularGiving.Total;
        var planId = totalAmount.Currency.Symbol + "-" + totalAmount.Amount.ToString("0.00");
        
        var getPlanRequest = new ApiGetPlanReq();
        getPlanRequest.Id = planId;

        try {
            var res = await _plansClient.
        } catch (ApiException apiException) {
            
        }
        
        throw new System.NotImplementedException();
    }

    private async Task<string> GetOrCreateProduct() {
        var request = new ApiCreateProductReq();
        request.Id = PayPalConstants.ProductId;
        request.Name = PayPalConstants.ProductId;
        request.Category = PayPalConstants.ProductCategory;
        request.Type = PayPalConstants.ProductType;
        request.Category = PayPalConstants.ProductCategory;

        var productId = string.Empty;

        try {
            var res = await _productsClient.CreateProduct(request);
            
            productId = res.Id;
        } catch (ApiException apiException) {
            var paypalError = apiException.Content.IfNotNull(JsonConvert.DeserializeObject<PayPalError>);

            var issue = paypalError?.Details[0]?.Issue;

            if (issue != null && issue != "DUPLICATE_RESOURCE_IDENTIFIER") {
                //throw some exception here since this is someother error
            }
        }
        

        return productId;
    }

    private async Task<string> CreateProduct() {
        var request = new ApiCreateProductReq();
        request.Id = PayPalConstants.ProductId;
        request.Name = PayPalConstants.ProductId;
        request.Category = PayPalConstants.ProductCategory;
        request.Type = PayPalConstants.ProductType;
        request.Category = PayPalConstants.ProductCategory;
        
        var res = await _productsClient.CreateProduct(request);
        
        return res.Id;
    }

    private async Task<string> GetOrCreatePlan(string planId) {
        
    }
    
    private async Task<string> CreatePlan() {
        var request = new ApiCreateProductReq();
        request.Id = PayPalConstants.ProductId;
        request.Name = PayPalConstants.ProductId;
        request.Category = PayPalConstants.ProductCategory;
        request.Type = PayPalConstants.ProductType;
        request.Category = PayPalConstants.ProductCategory;
        
        var res = await _productsClient.CreateProduct(request);
        
        return res.Id;
    }
}