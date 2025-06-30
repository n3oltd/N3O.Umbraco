using N3O.Umbraco.Cloud.Engage.Context;
using N3O.Umbraco.Entities;
using System;

namespace N3O.Umbraco.Crowdfunding;

public class ConnectCartIdAccessor : IConnectCartIdAccessor {
    private readonly Lazy<ConnectCartCookie> _connectCartCookie;
    private EntityId _connectCartId;

    public ConnectCartIdAccessor(Lazy<ConnectCartCookie> connectCartCookie) {
        _connectCartCookie = connectCartCookie;
    }

    public EntityId GetId() {
        if (_connectCartId == null) {
            _connectCartId = GetFromCookie();

            if (_connectCartId == null) {
                _connectCartCookie.Value.Reset();
                
                _connectCartId = GetFromCookie();
            }
        }

        return _connectCartId;
    }

    private EntityId GetFromCookie() {
        return EntityId.TryParse(_connectCartCookie.Value.GetValue());
    }
}
