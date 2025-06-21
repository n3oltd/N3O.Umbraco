using N3O.Umbraco.Cloud.Engage.Context;
using N3O.Umbraco.Entities;
using System;

namespace N3O.Umbraco.Cloud.Engage;

public class CrmCartIdAccessor : ICrmCartIdAccessor {
    private readonly Lazy<CrmCartCookie> _crmCartCookie;
    private EntityId _crmCartId;

    public CrmCartIdAccessor(Lazy<CrmCartCookie> crmCartCookie) {
        _crmCartCookie = crmCartCookie;
    }

    public EntityId GetId() {
        if (_crmCartId == null) {
            _crmCartId = GetFromCookie();

            if (_crmCartId == null) {
                _crmCartCookie.Value.Reset();
                
                _crmCartId = GetFromCookie();
            }
        }

        return _crmCartId;
    }

    private EntityId GetFromCookie() {
        return EntityId.TryParse(_crmCartCookie.Value.GetValue());
    }
}
