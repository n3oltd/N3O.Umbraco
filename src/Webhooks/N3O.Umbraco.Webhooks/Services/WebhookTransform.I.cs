using N3O.Umbraco.Entities;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Giving.Checkout {
    public interface IWebhookTransform<T>
        where T : Entity {
        object Transform(IJsonProvider jsonProvider, T entity);
    }
}