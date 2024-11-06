using N3O.Umbraco.Payments.PayPal.Clients.Models;
using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public interface IPlansClient {
    [Post("/v1/billing/plans")]
    Task<ApiCreatePlanRes> CreatePlanAsync(ApiCreatePlanReq req);
    
    [Get("/v1/billing/plans?product_id={req.productId}&page={req.pageNumber}")]
    Task<ApiGetPlansRes> GetPlansAsync(ApiGetPlansReq req);
}