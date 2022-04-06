using StackExchange.Profiling.Internal;
using System;
using System.Text;

namespace N3O.Umbraco.Utilities {
    public static class Base64 {
        public static string Encode(string plainText) {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Decode(string base64EncodedData) {
            try {
                if (!base64EncodedData.HasValue()) {
                    return null;
                }

                var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

                return Encoding.UTF8.GetString(base64EncodedBytes);
            } catch {
                return null;
            }
        }
    }
}