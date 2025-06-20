using System;

namespace N3O.Umbraco.Cloud.Models;

public interface IOrganizationInfo {
    string Name { get; }
    string CharityRegistration { get; }
    string AddressSingleLine { get; }
    string AddressPostalCode { get; }
    PublishedCountry AddressCountry { get; }
    Uri Logo { get; }
}