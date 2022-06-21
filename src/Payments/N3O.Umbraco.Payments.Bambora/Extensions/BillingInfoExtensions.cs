using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Bambora.Client;
using N3O.Umbraco.Payments.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Bambora.Extensions;

public static class BillingInfoExtensions {
    public static ApiBillingAddressReq GetApiBillingAddress(this BillingInfo billingInfo) {
        var address = billingInfo.Address;

        var billingAddress = new ApiBillingAddressReq();
        billingAddress.AddressLine1 = GetText(address.Line1, 50, true);
        billingAddress.AddressLine2 = GetText(address.Line2, 50, false);
        billingAddress.City = GetText(address.Locality, 40, true);
        billingAddress.EmailAddress = billingInfo.Email.Address;
        billingAddress.PostalCode = GetText(address.PostalCode, 10, true, "0000");
        billingAddress.Country = address.Country.Iso2Code;
        
        if (address.Country.Iso3Code.EqualsInvariant(BamboraConstants.Iso3CountryCodes.UnitedStates)) {
            billingAddress.Province = GetUsStateCode(address.AdministrativeArea);
        } else if (address.Country.Iso3Code.EqualsInvariant(BamboraConstants.Iso3CountryCodes.Canada)) {
            billingAddress.Province = GetCanadaProvinceCode(address.AdministrativeArea);
        } else {
            // As per Bambora documentation
            billingAddress.Province = "--";
        }

        return billingAddress;
    }

    private static string GetText(string value, int maxLength, bool required, string defaultValue = "--") {
        if (!value.HasValue() && required) {
            value = defaultValue;
        }

        if (value == null) {
            return null;
        }

        return value.RemoveNonAscii().Trim().Right(maxLength);
    }

    private static string GetCanadaProvinceCode(string administrativeArea) {
        if (CanadaProvinces.ContainsKey(administrativeArea)) {
            return administrativeArea.ToUpper();
        }

        foreach (var (code, name) in CanadaProvinces) {
            if (administrativeArea.Contains(name, StringComparison.InvariantCultureIgnoreCase)) {
                return code;
            }
        }

        // Safe default
        return "ON";
    }

    private static string GetUsStateCode(string administrativeArea) {
        if (UsStates.ContainsKey(administrativeArea)) {
            return administrativeArea.ToUpper();
        }

        foreach (var (code, name) in UsStates) {
            if (administrativeArea.Contains(name, StringComparison.InvariantCultureIgnoreCase)) {
                return code;
            }
        }

        // Safe default
        return "NY";
    }
    
    private static readonly Dictionary<string, string> CanadaProvinces = new(StringComparer.InvariantCultureIgnoreCase) {
        { "AB", "Alberta" },
        { "BC", "Columbia" },
        { "MB", "Manitoba" },
        { "NB", "Brunswick" },
        { "NL", "Newfoundland" },
        { "NS", "Nova Scotia" },
        { "NT", "Northwest" },
        { "NU", "Nunavut" },
        { "ON", "Ontario" },
        { "PE", "Edward" },
        { "QC", "Quebec" },
        { "SK", "Saskatchewan" },
        { "YT", "Yukon" }
    };

    private static readonly Dictionary<string, string> UsStates = new(StringComparer.InvariantCultureIgnoreCase) {
        { "AA", "Armed Forces America" },
        { "AE", "Armed Forces" },
        { "AK", "Alaska" },
        { "AL", "Alabama" },
        { "AP", "Armed Forces Pacific" },
        { "AR", "Arkansas" },
        { "AZ", "Arizona" },
        { "CA", "California" },
        { "CO", "Colorado" },
        { "CT", "Connecticut" },
        { "DC", "Washington DC" },
        { "DE", "Delaware" },
        { "FL", "Florida" },
        { "GA", "Georgia" },
        { "GU", "Guam" },
        { "HI", "Hawaii" },
        { "IA", "Iowa" },
        { "ID", "Idaho" },
        { "IL", "Illinois" },
        { "IN", "Indiana" },
        { "KS", "Kansas" },
        { "KY", "Kentucky" },
        { "LA", "Louisiana" },
        { "MA", "Massachusetts" },
        { "MD", "Maryland" },
        { "ME", "Maine" },
        { "MI", "Michigan" },
        { "MN", "Minnesota" },
        { "MO", "Missouri" },
        { "MS", "Mississippi" },
        { "MT", "Montana" },
        { "NC", "North Carolina" },
        { "ND", "North Dakota" },
        { "NE", "Nebraska" },
        { "NH", "New Hampshire" },
        { "NJ", "New Jersey" },
        { "NM", "New Mexico" },
        { "NV", "Nevada" },
        { "NY", "New York" },
        { "OH", "Ohio" },
        { "OK", "Oklahoma" },
        { "OR", "Oregon" },
        { "PA", "Pennsylvania" },
        { "PR", "Puerto Rico" },
        { "RI", "Rhode Island" },
        { "SC", "South Carolina" },
        { "SD", "South Dakota" },
        { "TN", "Tennessee" },
        { "TX", "Texas" },
        { "UT", "Utah" },
        { "VA", "Virginia" },
        { "VI", "Virgin Islands" },
        { "VT", "Vermont" },
        { "WA", "Washington" },
        { "WI", "Wisconsin" },
        { "WV", "West Virginia" },
        { "WY", "Wyoming" }
    };
}
