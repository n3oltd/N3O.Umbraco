using N3O.Umbraco.Payments.PayPal.Clients.Models;
using N3O.Umbraco.Payments.PayPal.Clients.PayPalErrors;
using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public interface IProductsClient {
    [Post("/v1/catalogs/products")]
    Task<ApiProductRes> CreateProduct(ApiCreateProductReq req);
    
    [Get("/v1/catalogs/products/{req.Id}")]
    Task<ApiProductRes> GetProduct(ApiGetProductReq req);
}