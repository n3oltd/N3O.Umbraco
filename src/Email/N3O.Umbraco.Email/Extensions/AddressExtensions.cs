using FluentEmail.Core.Models;
using N3O.Umbraco.Email.Models;

namespace N3O.Umbraco.Email.Extensions;

public static class AddressExtensions {
    public static EmailIdentity ToEmailIdentity(this Address address) {
        return new EmailIdentity(address.EmailAddress, address.Name);
    }
}