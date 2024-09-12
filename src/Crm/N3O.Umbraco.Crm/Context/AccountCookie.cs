using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Context;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;

namespace N3O.Umbraco.Crm.Context;

public class AccountCookie : Cookie {
    public AccountCookie(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

    public string GetId() {
        return Get()?.Id;
    }
    
    public string GetReference() {
        return Get()?.Reference;
    }
    
    public void Set(string id, string reference) {
        Set(new AccountIdReference { Id = id, Reference = reference });
    }
    
    protected override string GetDefaultValue() {
        return null;
    }

    protected override void SetOptions(CookieOptions cookieOptions) {
        base.SetOptions(cookieOptions);
        
        cookieOptions.HttpOnly = false;
    }

    private AccountIdReference Get() {
        var base64 = Base64.Decode(GetValue());
        
        return base64.IfNotNull(JsonConvert.DeserializeObject<AccountIdReference>);
    }

    private void Set(AccountIdReference idReference) {
        var base64 = Base64.Encode(JsonConvert.SerializeObject(idReference));
        
        SetValue(base64);
    }

    protected override string Name => "Account";
    protected override bool Session => true;
}
