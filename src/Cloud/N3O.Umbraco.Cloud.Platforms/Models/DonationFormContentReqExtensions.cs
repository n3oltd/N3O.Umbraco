using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Media;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public static class DonationFormContentReqExtensions {
    public static DonationFormContentReq ToDonationFormContentReq(this IHoldDonationFormContent src,
                                                                  IMediaUrl mediaUrl) {
        var donationFormContentReq = new DonationFormContentReq();
        donationFormContentReq.Summary = src.Summary;
        donationFormContentReq.Description = src.Description.ToHtmlString().ToRichTextContentReq();
        donationFormContentReq.Image = src.Image.ToImageSimpleContentReq(mediaUrl);
        donationFormContentReq.Icon = src.Icon.ToSvgContentReq(mediaUrl);

        return donationFormContentReq;
    }
}
