using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Strings;
using ElementType = N3O.Umbraco.Cloud.Platforms.Lookups.ElementType;
using PublishedElementType = N3O.Umbraco.Cloud.Platforms.Clients.ElementType;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class ElementPreviewTagGenerator : PreviewTagGenerator {
    private readonly IContentLocator _contentLocator;
    private readonly IUmbracoMapper _mapper;
    private readonly IMarkupEngine _markupEngine;
    
    protected ElementPreviewTagGenerator(ICdnClient cdnClient,
                                         IJsonProvider jsonProvider,
                                         IContentLocator contentLocator,
                                         IUmbracoMapper mapper,
                                         IMarkupEngine markupEngine)
        : base(cdnClient, jsonProvider) {
        _contentLocator = contentLocator;
        _mapper = mapper;
        _markupEngine = markupEngine;
    }
    
    protected abstract ElementType ElementType { get; }

    protected override string ContentTypeAlias => ElementType.ContentTypeAlias;

    protected override void PopulatePreviewData(IReadOnlyDictionary<string, object> content,
                                                Dictionary<string, object> previewData) {
        var publishedDonationForm = new PublishedDonationForm();
        publishedDonationForm.Id = content[AliasHelper<ElementContent>.PropertyAlias(x => x.Key)].ToString();
        publishedDonationForm.Type = ElementType.ToEnum<PublishedElementType>();

        var campaignUdi = content[AliasHelper<ElementContent>.PropertyAlias(x => x.Campaign)]?.ToString();
        
        if (campaignUdi.HasValue()) {
            var campaign = _contentLocator.ById<CampaignContent>(UdiParser.Parse(campaignUdi).ToId().Value);
            
            publishedDonationForm.Campaign = _mapper.Map<CampaignContent, PublishedCampaignSummary>(campaign);
            publishedDonationForm.Designation = _mapper.Map<DesignationContent, PublishedDesignation>(campaign.DefaultDesignation);
            
            publishedDonationForm.Designation.ShortDescription = _markupEngine.RenderHtml(publishedDonationForm.Designation.ShortDescription).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
            publishedDonationForm.Designation.LongDescription = _markupEngine.RenderHtml(publishedDonationForm.Designation.LongDescription).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
        } else {
            var defaultCampaign = _contentLocator.Single<PlatformsContent>().Campaigns.First();
            
            publishedDonationForm.Designation = _mapper.Map<DesignationContent, PublishedDesignation>(defaultCampaign.DefaultDesignation);
        }

        previewData["publishedForm"] = publishedDonationForm;
    }
}