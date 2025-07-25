﻿using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using System;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedDesignationMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;

    public PublishedDesignationMapping(IMediaUrl mediaUrl) {
        _mediaUrl = mediaUrl;
        _mediaUrl = mediaUrl;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DesignationContent, PublishedDesignation>((_, _) => new PublishedDesignation(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(DesignationContent src, PublishedDesignation dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = src.Type.ToEnum<DesignationType>();
        dest.Name = src.Name;
        dest.Image = _mediaUrl.GetMediaUrl(src.Image, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        dest.Icon = _mediaUrl.GetMediaUrl(src.Icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        dest.ShortDescription = src.ShortDescription.ToHtmlString();
        dest.LongDescription = src.LongDescription.ToHtmlString();
        dest.GiftTypes = src.GetGiftTypes().Select(x => x.ToEnum<GiftType>().GetValueOrThrow()).ToList();
        dest.SuggestedGiftType = src.SuggestedGiftType.ToEnum<GiftType>();
        dest.FundDimensions = src.ToPublishedDesignationFundDimensions();
        dest.Fund = src.Fund.IfNotNull(ctx.Map<FundDesignationContent, PublishedFundDesignation>);
        dest.Feedback = src.Feedback.IfNotNull(ctx.Map<FeedbackDesignationContent, PublishedFeedbackDesignation>);
        dest.Sponsorship = src.Sponsorship.IfNotNull(ctx.Map<SponsorshipDesignationContent, PublishedSponsorshipDesignation>);
    }
}