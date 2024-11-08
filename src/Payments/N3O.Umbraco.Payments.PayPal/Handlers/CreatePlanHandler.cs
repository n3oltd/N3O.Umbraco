using N3O.Umbraco.Content;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Checkout;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.PayPal.Clients;
using N3O.Umbraco.Payments.PayPal.Clients.Models;
using N3O.Umbraco.Payments.PayPal.Commands;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanRes;
using Newtonsoft.Json;
using NUglify.Helpers;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Handlers;

public class CreatePlanHandler : IRequestHandler<CreatePlanCommand, None, PayPalCreatePlanRes>{
    private readonly ICheckoutAccessor _checkoutAccessor;
    private readonly IContentCache _contentCache;
    private readonly IPlansClient _plansClient;
    private readonly IProductsClient _productsClient;

    public CreatePlanHandler(ICheckoutAccessor checkoutAccessor,
                             IContentCache contentCache,
                             IPlansClient plansClient,
                             IProductsClient productsClient) {
        _checkoutAccessor = checkoutAccessor;
        _contentCache = contentCache;
        _plansClient = plansClient;
        _productsClient = productsClient;
    }
    
    public async Task<PayPalCreatePlanRes> Handle(CreatePlanCommand request, CancellationToken cancellationToken) {
        var checkout = await _checkoutAccessor.GetAsync(cancellationToken);
        var totalAmount = checkout.RegularGiving.Total;
        var planName = totalAmount.Currency.Symbol + "-" + totalAmount.Amount.ToString("0.00");

        var productId = await GetOrCreateProduct();
        
        var planId = await GetOrCreatePlan(productId, planName, totalAmount);
        
        return new PayPalCreatePlanRes() { PlanId = planId };
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
            var res = await _productsClient.CreateProductAsync(request);
            
            productId = res.Id;
        } catch (ApiException apiException) {
            var paypalError = apiException.Content.IfNotNull(JsonConvert.DeserializeObject<PayPalError>);

            var issue = paypalError?.Details[0]?.Issue;

            if (issue != null && issue == "DUPLICATE_RESOURCE_IDENTIFIER") {
                productId = request.Id;
            } else {
                throw new Exception(); //TODO we can change this to a specific exception
            }
        }
        
        return productId;
    }

    private async Task<string> GetOrCreatePlan(string productId, string planName, Money totalAmount) {
        //TODO caching can be used here when retrieving the plans
        var planId = string.Empty;
        var page = 1;
        var foundPlan = false;
        
        do {
            var request = new ApiGetPlansReq();
            request.ProductId = productId;
            request.PageNumber = page.ToString();
            request.TotalRequired = true;
            
            var res = await _plansClient.GetPlansAsync(request);
            
            if (res.TotalItems == 0) {
                planId = await CreatePlan(productId, planId, totalAmount);
                
                break;
            } else {
                foreach (var plan in res.Plans.Where(x => x.Status == PayPalConstants.PlanStatus)) {
                    if (plan.Name != planName) continue;

                    planId = plan.Id;
                    foundPlan = true;
                    
                    break;
                }

                if (foundPlan) break;
            }

            if (page == res.TotalPages) {
                planId = await CreatePlan(productId, planName, totalAmount);

                break;
            }
            
            page++;
        } while (true);
        
        return planId;
    }
    
    private async Task<string> CreatePlan(string productId, string planName, Money totalAmount) {
        var fixedPrice = new FixedPrice();
        fixedPrice.Value = totalAmount.Amount.ToString("0.00");
        fixedPrice.CurrencyCode = totalAmount.Currency.Code;
        
        var pricingScheme = new PricingScheme();
        pricingScheme.FixedPrice = fixedPrice;
        
        var frequency = new Frequency();
        frequency.IntervalCount = PayPalConstants.FrequencyCount;
        frequency.IntervalUnit = PayPalConstants.FrequencyInterval;
        
        var billingCycle = new BillingCycle();
        billingCycle.Frequency = frequency;
        billingCycle.TenureType = PayPalConstants.TenureType;
        billingCycle.Sequence = PayPalConstants.BillingCycleSequence;
        billingCycle.TotalCycles = PayPalConstants.BillingCycleTotalCycles;
        billingCycle.PricingScheme = pricingScheme;

        var billingCycles = new List<BillingCycle>(){ billingCycle };
        
        var paymentPreferences = new PaymentPreferences();
        paymentPreferences.AutoBillOutstanding = true;
        paymentPreferences.SetupFeeFailureAction = PayPalConstants.SetupFeeFailureAction;
        paymentPreferences.PaymentFailureThreshold = PayPalConstants.PaymentFailureThreshold;
        
        var request = new ApiCreatePlanReq();
        request.ProductId = productId;
        request.Name = planName;
        request.Status = PayPalConstants.PlanStatus;
        request.BillingCycles = billingCycles;
        request.PaymentPreferences = paymentPreferences;

        try {
            var res = await _plansClient.CreatePlanAsync(request);
            
            return res.Id;
        } catch (ApiException apiException) {
            var exception = new Exception(apiException.Message);
            
            return string.Empty;
        }
    }
}