using BitArmory.ReCaptcha;
using N3O.Umbraco.Captcha.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Captcha;

public class CaptchaV3Validator : ICaptchaValidator {
    private readonly IContentCache _contentCache;
    private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;

    public CaptchaV3Validator(IContentCache contentCache, IRemoteIpAddressAccessor remoteIpAddressAccessor) {
        _contentCache = contentCache;
        _remoteIpAddressAccessor = remoteIpAddressAccessor;
    }
    
    public bool CanValidate => _contentCache.Single<CaptchaV3SettingsContent>().HasValue();

    public async Task<bool> IsValidAsync(string token,
                                         string action = null,
                                         CancellationToken cancellationToken = default) {
        if (!token.HasValue()) {
            return false;
        }
        
        var settings = _contentCache.Single<CaptchaV3SettingsContent>();
        var remoteIp = _remoteIpAddressAccessor.GetRemoteIpAddress();

        var captchaApi = new ReCaptchaService();
        var result = await captchaApi.Verify3Async(token,
                                                   remoteIp.ToString(),
                                                   settings.SecretKey,
                                                   cancellationToken);

        if (!result.IsSuccess || result.Action != action || result.Score < settings.Threshold) {
            return false;
        } else {
            return true;
        }
    }
}
