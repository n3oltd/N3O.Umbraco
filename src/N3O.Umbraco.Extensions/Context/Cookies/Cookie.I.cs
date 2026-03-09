using Microsoft.AspNetCore.Http;

namespace N3O.Umbraco.Context;

public interface ICookie : IReadOnlyCookie {
    void SetValue(string value);
    void Write(IResponseCookies responseCookies);
}
