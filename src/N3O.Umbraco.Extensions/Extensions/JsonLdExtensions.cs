using N3O.Umbraco.Lookups;
using N3O.Umbraco.StructuredData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Extensions;

public static class JsonLdExtensions {
    public static JsonLd Id(this JsonLd jsonLd, string id) {
        jsonLd.Custom("@id", id);

        return jsonLd;
    }

    public static JsonLd Name(this JsonLd jsonLd, string name) {
        jsonLd.Custom("name", name);

        return jsonLd;
    }

    public static JsonLd Headline(this JsonLd jsonLd, string headline) {
        jsonLd.Custom("headline", headline);

        return jsonLd;
    }

    public static JsonLd Description(this JsonLd jsonLd, string description) {
        jsonLd.Custom("description", description);

        return jsonLd;
    }

    public static JsonLd Image(this JsonLd jsonLd, string image) {
        jsonLd.Custom("image", image);

        return jsonLd;
    }

    public static JsonLd InLanguage(this JsonLd jsonLd, string language) {
        jsonLd.Custom("inLanguage", language);

        return jsonLd;
    }

    public static JsonLd DatePublished(this JsonLd jsonLd, DateTime date) {
        jsonLd.Custom("datePublished", date.ToString("yyyy-MM-dd"));

        return jsonLd;
    }

    public static JsonLd DateModified(this JsonLd jsonLd, DateTime date) {
        jsonLd.Custom("dateModified", date.ToString("yyyy-MM-dd"));

        return jsonLd;
    }

    public static JsonLd SameAs(this JsonLd jsonLd, IEnumerable<string> urls) {
        var list = urls.OrEmpty().Where(x => x.HasValue()).ToList();

        if (list.Any()) {
            jsonLd.Custom("sameAs", list);
        }

        return jsonLd;
    }

    public static JsonLd Reference(this JsonLd jsonLd, string key, string id) {
        var reference = JsonLd.New();

        reference.Id(id);
        jsonLd.Custom(key, reference);

        return jsonLd;
    }

    public static JsonLd TranslationOfWork(this JsonLd jsonLd, string id) {
        return jsonLd.Reference("translationOfWork", id);
    }

    public static JsonLd WorkTranslation(this JsonLd jsonLd, string id) {
        return jsonLd.Reference("workTranslation", id);
    }

    public static JsonLd Url(this JsonLd jsonLd, string url) {
        jsonLd.Custom("url", url);

        return jsonLd;
    }

    public static JsonLd StartDate(this JsonLd jsonLd, DateTime date) {
        jsonLd.Custom("startDate", date.ToString("yyyy-MM-dd"));

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
        return Country(jsonLd, country.Iso3Code);
    }

    public static JsonLd Country(this JsonLd jsonLd, string iso3Code) {
        jsonLd.Custom("addressCountry", iso3Code);

        return jsonLd;
    }
}
