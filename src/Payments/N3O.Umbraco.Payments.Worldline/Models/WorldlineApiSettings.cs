using N3O.Umbraco.Payments.Worldline.Lookups;
using System;

namespace N3O.Umbraco.Payments.Worldline.Models;

public class WorldlineApiSettings : Value {
    public WorldlineApiSettings(WorldlinePlatform platform,
                                Uri endpoint,
                                string apiKey,
                                string apiSecret,
                                string merchantId) {
        Platform = platform;
        Endpoint = endpoint;
        ApiKey = apiKey;
        ApiSecret = apiSecret;
        MerchantId = merchantId;
    }

    public WorldlinePlatform Platform { get; }
    public Uri Endpoint { get; }
    public string ApiKey { get; }
    public string ApiSecret { get; }
    public string MerchantId { get; }
}
