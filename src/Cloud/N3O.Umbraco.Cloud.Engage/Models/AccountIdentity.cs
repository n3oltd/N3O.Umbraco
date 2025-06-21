using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class AccountIdentity : Value {
    public AccountIdentity(string id, string reference, string token) {
        Id = id;
        Reference = reference;
        Token = token;
    }

    public string Id { get; }
    public string Reference { get; }
    public string Token { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
        yield return Reference;
        yield return Token;
    }
}