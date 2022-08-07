using N3O.Umbraco.Payments.Worldline.Lookups;

namespace N3O.Umbraco.Payments.Worldline.Models;

public class WorldlineViewModel : IPaymentMethodViewModel<WorldlinePaymentMethod> {
    public WorldlineViewModel(WorldlineApiSettings apiSettings) {
        Platform = apiSettings.Platform;
        Endpoint = apiSettings.Endpoint.ToString();
    }

    public WorldlinePlatform Platform { get; }
    public string Endpoint { get; }
}
