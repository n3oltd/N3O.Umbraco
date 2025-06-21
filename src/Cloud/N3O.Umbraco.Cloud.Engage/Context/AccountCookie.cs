using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Cloud.Engage.Models;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;

namespace N3O.Umbraco.Cloud.Engage.Context;

public class AccountCookie : Cookie {
    public AccountCookie(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

    public string GetId() {
        return Get()?.Id;
    }
    
    public string GetReference() {
        return Get()?.Reference;
    }
    
    public string GetToken() {
        return Get()?.Token;
    }
    
    public void Set(string id, string reference, string token) {
        Set(new AccountIdentity(id, reference, token));
    }
    
    protected override string GetDefaultValue() {
        return null;
    }

    protected override void SetOptions(CookieOptions cookieOptions) {
        base.SetOptions(cookieOptions);
        
        cookieOptions.HttpOnly = false;
    }

    private AccountIdentity Get() {
        var base64 = Base64.Decode(GetValue());
        
        return base64.IfNotNull(JsonConvert.DeserializeObject<AccountIdentity>);
    }

    private void Set(AccountIdentity accountIdentity) {
        var base64 = Base64.Encode(JsonConvert.SerializeObject(accountIdentity));
        
        SetValue(base64);
    }

    protected override string Name => "Account";
    protected override bool Session => true;
}
