using Humanizer.Bytes;
using MimeKit;
using MimeKit.IO;
using System.IO;

namespace N3O.Umbraco.Email.Extensions;

public static class MimeMessageExtensions {
    public static MemoryStream ToStream(this MimeMessage message) {
        var stream = new MemoryStream();

        message.WriteTo(stream);

        return stream;
    }

    public static ByteSize TotalSize(this MimeMessage message) {
        var totalLength = 0L;

        foreach (var attachment in message.Attachments) {
            using (var stream = new MeasuringStream()) {
                var rfc822 = attachment as MessagePart;
                var part = attachment as MimePart;

                if (rfc822 != null) {
                    rfc822.Message.WriteTo(stream);
                } else {
                    part.Content.DecodeTo(stream);
                }

                totalLength += stream.Length;
            }
        }

        return ByteSize.FromBytes(totalLength);
    }
}
