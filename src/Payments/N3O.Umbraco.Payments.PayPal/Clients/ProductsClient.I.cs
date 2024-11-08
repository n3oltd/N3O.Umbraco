using N3O.Umbraco.Payments.PayPal.Clients.Models;
using Refit;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public interface IProductsClient {
    [Post("/v1/catalogs/products")]
    Task<ApiProductRes> CreateProductAsync(ApiCreateProductReq req);
}