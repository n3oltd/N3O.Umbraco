using System;
using System.Net;

namespace N3O.Umbraco.Blocks.Exceptions;

// Markup is returned with a 200 and rendered in the preview frame, which is a document of its own that the
// backoffice plugin's stylesheet does not reach, so each banner is a self-contained document carrying its own
// styling.
public abstract class BlockPreviewException : Exception {
    protected BlockPreviewException(string message) : base(message) { }

    public abstract string Markup { get; }

    protected string GetBannerMarkup(string backgroundColor) {
        return "<!DOCTYPE html><html><head><meta charset=\"utf-8\"></head>" +
               "<body style=\"margin:0;font-family:sans-serif;font-size:14px\">" +
               $"<div style=\"background-color:{backgroundColor};color:#fff;padding:8px 14px\">" +
               $"{WebUtility.HtmlEncode(Message)}</div></body></html>";
    }
}

public class BlockPreviewErrorException : BlockPreviewException {
    public BlockPreviewErrorException(string message) : base(message) { }

    public override string Markup => GetBannerMarkup("#d42054");
}

public class BlockPreviewWarningException : BlockPreviewException {
    public BlockPreviewWarningException(string message) : base(message) { }

    public override string Markup => GetBannerMarkup("#f0ac00");
}
