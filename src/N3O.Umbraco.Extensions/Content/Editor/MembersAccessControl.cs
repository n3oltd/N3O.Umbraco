using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Content;

public abstract class MembersAccessControl : ContentAccessControl {
    private readonly IContentHelper _contentHelper;
    private readonly IDataTypeService _dataTypeService;
    private readonly IMemberManager _memberManager;

    protected MembersAccessControl(IContentHelper contentHelper,
                                   IDataTypeService dataTypeService,
                                   IMemberManager memberManager)
        : base(contentHelper) {
        _contentHelper = contentHelper;
        _dataTypeService = dataTypeService;
        _memberManager = memberManager;
    }

    protected override bool IsAccessControlFor(string contentTypeAlias) {
        return contentTypeAlias.EqualsInvariant(ContentTypeAlias);
    }

    protected override async Task<bool> AllowEditAsync(ContentProperties contentProperties) {
        var property = contentProperties.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(PropertyAlias));
        var maxValues = GetMaxValues(_dataTypeService.GetDataType(property.Type.DataTypeId));
        
        if (maxValues == 1) {
            return await AllowEditAsync(() => _contentHelper.GetPickerValue<IPublishedContent>(contentProperties,
                                                                                               PropertyAlias).Yield());
        } else {
            return await AllowEditAsync(() => _contentHelper.GetPickerValues<IPublishedContent>(contentProperties,
                                                                                                PropertyAlias));
        }
    }

    protected override async Task<bool> AllowEditAsync(IPublishedContent content) {
        var property = content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(PropertyAlias));
        var maxValues = GetMaxValues(_dataTypeService.GetDataType(property.PropertyType.DataType.Id));
        
        if (maxValues == 1) {
            return await AllowEditAsync(() => ((IPublishedContent) property.GetValue(PropertyAlias)).Yield());
            
        } else {
            return await AllowEditAsync(() => (IEnumerable<IPublishedContent>) property.GetValue(PropertyAlias));
        }
    }
    
    private async Task<bool> AllowEditAsync(Func<IEnumerable<IPublishedContent>> getAllowedMembers) {
        var member = await _memberManager.GetCurrentPublishedMemberAsync();

        if (member == null) {
            return false;
        }

        var allowedMembers = getAllowedMembers().OrEmpty();

        return allowedMembers.Any(x => x.Key == member.Key);
    }
    
    private int GetMaxValues(IDataType dataType) {
        var configuration = dataType.ConfigurationAs<MultiNodePickerConfiguration>();
        
        return configuration.MaxNumber;
    }
    
    protected abstract string ContentTypeAlias { get; }
    protected abstract string PropertyAlias { get; }
}