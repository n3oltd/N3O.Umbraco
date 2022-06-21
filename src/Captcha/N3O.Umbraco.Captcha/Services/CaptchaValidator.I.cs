using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Captcha;

public interface ICaptchaValidator {
    Task<bool> IsValidAsync(string token, string action, CancellationToken cancellationToken = default);
}
