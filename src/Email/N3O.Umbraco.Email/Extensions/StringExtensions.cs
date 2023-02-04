using MimeKit;

namespace N3O.Umbraco.Email.Extensions;

public static class StringExtensions {
    public static ContentType ToContentType(this string value) {
        var bits = value.Split('/');

        var contentType = new ContentType(bits[0], bits[1]);

        return contentType;
    }
}