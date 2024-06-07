using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Captcha;

public interface ICaptchaValidator {
    bool CanValidate  { get; }
    Task<bool> IsValidAsync(string token, string action = null, CancellationToken cancellationToken = default);
}
