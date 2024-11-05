using Refit;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public interface IPlansClient {
    [Post("/v1/billing/plans")]
    ApiCreatePlanRes CreatePlan(ApiCreatePlanReq req);
    
    [Get("/v1/billing/plans/{req.planId}")]
    ApiCreatePlanRes GetPlan(ApiCreatePlanReq req);
}