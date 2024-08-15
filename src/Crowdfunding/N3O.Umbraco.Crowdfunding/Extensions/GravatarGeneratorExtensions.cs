using Flurl;
using System;
using System.Security.Cryptography;
using System.Text;

namespace N3O.Umbraco.CrowdFunding.Extensions;

public static class GravatarGeneratorExtensions {
    private const string GravatarBaseUrl = "https://www.gravatar.com/avatar";
    
    public static string GetGravatarUrl(this string email, int size = 80) {
        var emailHash = GetMd5Hash(email);
        
        var url = new Url(GravatarBaseUrl);
        url.AppendPathSegment(emailHash);
        url.SetQueryParam("s", size);
        url.SetQueryParam("d", "identicon");
        url.SetQueryParam("r", "g");

        return url;
    }

    private static string GetMd5Hash(string email) {
        using (var md5 = MD5.Create()) {
            var inputBytes = Encoding.ASCII.GetBytes(email);
            var hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}