using Refit;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public interface IPlansClient {
    [Post("/v1/billing/plans")]
    ApiPlanRes CreatePlan(ApiPlanReq req);
}