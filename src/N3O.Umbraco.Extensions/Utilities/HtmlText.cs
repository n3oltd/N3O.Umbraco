using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace N3O.Umbraco.Utilities;

public static class HtmlText {
    public static HtmlString BreakWords(string s, int maxLineLength, string brClass = null) {
        var sb = new StringBuilder();
        var lineLength = 0;

        foreach (var word in s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)) {
            if (lineLength != 0 && (lineLength + word.Length) > maxLineLength) {
                sb.Append("<br");

                if (brClass != null) {
                    sb.Append(" class=\"" + brClass + "\"");
                }

                sb.Append(">");

                lineLength = 0;
            }

            sb.Append(word + " ");

            lineLength += word.Length;
        }

        return new HtmlString(sb.ToString());
    }

    public static HtmlString BrList(IEnumerable<string> strings) {
        if (strings == null) {
            return null;
        }

        return Nl2Br(string.Join("\n", strings));
    }

    public static HtmlString Nl2Br(this string str) {
        if (str == null) {
            return null;
        }

        return new HtmlString(HttpUtility.HtmlEncode(str).Replace("\n", "<br />").Replace("\r\n", "<br />"));
    }
}
