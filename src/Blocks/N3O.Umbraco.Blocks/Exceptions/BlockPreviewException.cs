using System;

namespace N3O.Umbraco.Blocks.Exceptions;

public abstract class BlockPreviewException : Exception {
    protected BlockPreviewException(string message) : base(message) { }
    
    public abstract string Markup { get; }
}

public class BlockPreviewErrorException : BlockPreviewException {
    public BlockPreviewErrorException(string message) : base(message) { }

    public override string Markup => $"<div class=\"preview-alert preview-alert-error\">{Message}</div>";
}

public class BlockPreviewWarningException : BlockPreviewException {
    public BlockPreviewWarningException(string message) : base(message) { }

    public override string Markup => $"<div class=\"preview-alert preview-alert-warning\">{Message}</div>";
}