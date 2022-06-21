using System.Net;

namespace N3O.Umbraco.Context;

public interface IRemoteIpAddressAccessor {
    IPAddress GetRemoteIpAddress();
}
