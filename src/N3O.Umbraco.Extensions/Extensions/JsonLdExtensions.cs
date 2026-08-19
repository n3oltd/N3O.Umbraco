using N3O.Umbraco.Lookups;
using N3O.Umbraco.StructuredData;
using System;
using System.Globalization;

namespace N3O.Umbraco.Extensions;

public static class JsonLdExtensions {
    public static JsonLd Name(this JsonLd jsonLd, string name) {
        jsonLd.Custom("name", name);

        return jsonLd;
    }

    public static JsonLd Url(this JsonLd jsonLd, string url) {
        jsonLd.Custom("url", url);

        return jsonLd;
    }

    public static JsonLd StartDate(this JsonLd jsonLd, DateTime date) {
        jsonLd.Custom("startDate", date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

        return jsonLd;
    }

    public static JsonLd Email(this JsonLd jsonLd, string email) {
        jsonLd.Custom("email", email);

        return jsonLd;
    }

    public static JsonLd Telephone(this JsonLd jsonLd, string telephone) {
        jsonLd.Custom("telephone", telephone);

        return jsonLd;
    }

    public static JsonLd Address(this JsonLd jsonLd, string address) {
        jsonLd.Custom("streetAddress", address);

        return jsonLd;
    }

    public static JsonLd PostalCode(this JsonLd jsonLd, string postalCode) {
        jsonLd.Custom("postalCode", postalCode);

        return jsonLd;
    }

    public static JsonLd Country(this JsonLd jsonLd, Country country) {
        return Country(jsonLd, country.Iso2Code);
    }

    public static JsonLd Country(this JsonLd jsonLd, string countryCode) {
        jsonLd.Custom("addressCountry", countryCode);

        return jsonLd;
    }
}
