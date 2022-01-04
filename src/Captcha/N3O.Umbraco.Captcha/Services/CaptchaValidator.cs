using BitArmory.ReCaptcha;
using N3O.Umbraco.Captcha.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Captcha {
    public class CaptchaValidator : ICaptchaValidator {
        private readonly IContentCache _contentCache;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;

        public CaptchaValidator(IContentCache contentCache, IRemoteIpAddressAccessor remoteIpAddressAccessor) {
            _contentCache = contentCache;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
        }

        public async Task<bool> IsValidAsync(string token,
                                             string action,
                                             CancellationToken cancellationToken = default) {
            var settings = _contentCache.Single<CaptchaSettings>();
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
}