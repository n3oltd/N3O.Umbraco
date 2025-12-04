/*using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Markup;
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
                                         IMarkupEngine markupEngine,
                                         ILookups lookups)
        : base(cdnClient, jsonProvider, lookups) {
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
        var offeringUdi = content[AliasHelper<DesignatableElementContent<DonationFormElementContent>>.PropertyAlias(x => x.Offering)]?.ToString();
        
        if (campaignUdi.HasValue()) {
            var campaign = _contentLocator.ById<CampaignContent>(UdiParser.Parse(campaignUdi).ToId().Value);
            
            publishedDonationForm.Campaign = _mapper.Map<CampaignContent, PublishedCampaignSummary>(campaign);
            publishedDonationForm.Offering = _mapper.Map<OfferingContent, PublishedOffering>(campaign.DefaultOffering);
            
            publishedDonationForm.Offering.ShortDescription = _markupEngine.RenderHtml(publishedDonationForm.Offering.ShortDescription).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
            publishedDonationForm.Offering.LongDescription = _markupEngine.RenderHtml(publishedDonationForm.Offering.LongDescription).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
        } else if (offeringUdi.HasValue()) {
            var offering = _contentLocator.ById<OfferingContent>(UdiParser.Parse(offeringUdi).ToId().Value);
            var campaign = offering.Content().Parent.As<CampaignContent>();
            
            publishedDonationForm.Campaign = _mapper.Map<CampaignContent, PublishedCampaignSummary>(campaign);
            publishedDonationForm.Offering = _mapper.Map<OfferingContent, PublishedOffering>(offering);
            
            publishedDonationForm.Offering.ShortDescription = _markupEngine.RenderHtml(publishedDonationForm.Offering.ShortDescription).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
            publishedDonationForm.Offering.LongDescription = _markupEngine.RenderHtml(publishedDonationForm.Offering.LongDescription).IfNotNull(x => new HtmlEncodedString(x.ToString())).ToHtmlString();
        }  else {
            var defaultCampaign = _contentLocator.Single<PlatformsContent>().Campaigns.First();
            
            publishedDonationForm.Offering = _mapper.Map<OfferingContent, PublishedOffering>(defaultCampaign.DefaultOffering);
        }
            
        publishedDonationForm.Dimension1 = GetDataListValue<FundDimension1Value>(content, AliasHelper<DesignatableElementContent<DonationFormElementContent>>.PropertyAlias(x => x.Dimension1))?.Name;
        publishedDonationForm.Dimension2 = GetDataListValue<FundDimension2Value>(content, AliasHelper<DesignatableElementContent<DonationFormElementContent>>.PropertyAlias(x => x.Dimension2))?.Name;
        publishedDonationForm.Dimension3 = GetDataListValue<FundDimension3Value>(content, AliasHelper<DesignatableElementContent<DonationFormElementContent>>.PropertyAlias(x => x.Dimension3))?.Name;
        publishedDonationForm.Dimension4 = GetDataListValue<FundDimension4Value>(content, AliasHelper<DesignatableElementContent<DonationFormElementContent>>.PropertyAlias(x => x.Dimension4))?.Name;

        previewData["publishedForm"] = publishedDonationForm;
    }
}*/