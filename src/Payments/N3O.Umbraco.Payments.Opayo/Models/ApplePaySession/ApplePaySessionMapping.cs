using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Opayo.Models;

public class ApplePaySessionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IApplePaySession, ApplePaySessionRes>((_, _) => new ApplePaySessionRes(), Map);
    }
    
    private void Map(IApplePaySession src, ApplePaySessionRes dest, MapperContext ctx) {
        dest.Status = src.Status;
        dest.StatusCode = src.StatusCode;
        dest.StatusDetail = src.StatusDetail;
        dest.EpochTimeStamp = src.EpochTimeStamp;
        dest.ExpiresAt = src.ExpiresAt;
        dest.MerchantSessionIdentifier = src.MerchantSessionIdentifier;
        dest.Nonce = src.Nonce;
        dest.MerchantIdentifier = src.MerchantIdentifier;
        dest.DomainName = src.DomainName;
        dest.DisplayName = src.DisplayName;
        dest.Signature = src.Signature;
        dest.SessionValidationToken = src.SessionValidationToken;
    }
}