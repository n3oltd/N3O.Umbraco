using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using UmbracoUdiEntityType = Umbraco.Cms.Core.Constants.UdiEntityType;

namespace N3O.Umbraco.Content;

public class ContentPickerPropertyBuilder : PropertyBuilder {
    private static readonly string Document = UmbracoUdiEntityType.Document;
    private static readonly string Member = UmbracoUdiEntityType.Member;
    
    public ContentPickerPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void SetContent(IEnumerable<IContent> values) {
        SetContent(values.OrEmpty().ToArray());
    }

    public void SetContent(params IContent[] values) {
        SetContent(values.ExceptNull().Select(x => x.Key).ToArray());
    }

    public void SetContent<T>(IEnumerable<T> values) where T : IUmbracoContent {
        SetContent(values.OrEmpty().ToArray());
    }

    public void SetContent<T>(params T[] values) where T : IUmbracoContent {
        SetContent(values.Select(x => x?.Content()).ExceptNull().Select(x => x.Key).ToArray());
    }

    public void SetContent(IEnumerable<IPublishedContent> values) {
        SetContent(values.OrEmpty().ToArray());
    }

    public void SetContent(params IPublishedContent[] values) {
        SetContent(values.ExceptNull().Select(x => x.Key).ToArray());
    }

    public void SetContent(IEnumerable<Guid> values) {
        SetContent(values.OrEmpty().ToArray());
    }
    
    public void SetContent(params Guid[] values) {
        var documentUdis = values.ExceptNull().Select(x => Udi.Create(Document, x).ToString()).ToList();

        if (documentUdis.IsSingle()) {
            Value = documentUdis.Single();
        } else {
            Value = string.Join(",", documentUdis);
        }
    }
    
    public void SetMember(params IMember[] values) {
        SetMember(values.ExceptNull().Select(x => x.Key).ToArray());
    }
    
    public void SetMember(params IPublishedContent[] values) {
        SetMember(values.ExceptNull().Select(x => x.Key).ToArray());
    }

    public void SetMember(IEnumerable<Guid> values) {
        SetMember(values.OrEmpty().ToArray());
    }
    
    public void SetMember(params Guid[] values) {
        var documentUdis = values.ExceptNull().Select(x => Udi.Create(Member, x).ToString()).ToList();

        if (documentUdis.IsSingle()) {
            Value = documentUdis.Single();
        } else {
            Value = string.Join(",", documentUdis);
        }
    }
}
