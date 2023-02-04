using FluentEmail.Core.Models;
using N3O.Umbraco.Email.Models;
using System.IO;

namespace N3O.Umbraco.Email.Extensions;

public static class AttachmentExtensions {
    public static EmailAttachment ToEmailAttachment(this Attachment attachment) {
        using (var memoryStream = new MemoryStream()) {
            attachment.Data.CopyTo(memoryStream);

            return new EmailAttachment(attachment.Filename,
                                       attachment.ContentType,
                                       memoryStream.ToArray());
        }
    }
}