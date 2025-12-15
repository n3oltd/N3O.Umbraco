using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Cloud.Models;

public interface IOrganizationInfo {
    string Name { get; }
    string RegistrationDetails { get; }
    string AddressSingleLine { get; }
    string AddressPostalCode { get; }
    Country AddressCountry { get; }
    Uri Logo { get; }
}