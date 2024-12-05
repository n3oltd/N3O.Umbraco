using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Extensions;
using N3O.Umbraco.Elements.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Elements.Content;

public class DonationCategoryContent : UmbracoContent<DonationCategoryContent> {
    private static readonly string DimensionAlias = AliasHelper<DimensionDonationCategoryContent>.ContentTypeAlias();
    private static readonly string EphemeralAlias = AliasHelper<EphemeralDonationCategoryContent>.ContentTypeAlias();
    private static readonly string GeneralAlias = AliasHelper<GeneralDonationCategoryContent>.ContentTypeAlias();
    
    public Guid Id => Content().Key;
    public Guid? ParentId => Content().Parent?.Key;
    public string Name => Content().Name;
    public MediaWithCrops Image => GetValue(x => x.Image);

    public string ImageUrl => Image.GetCropUrl(ElementsConstants.DonationCategory.CropAlias);
    
    public override void Content(IPublishedContent content) {
        base.Content(content);
        
        if (Type == DonationCategoryTypes.Dimension) {
            Dimension = new DimensionDonationCategoryContent();
            Dimension.Content(content);
        } else if (Type == DonationCategoryTypes.Ephemeral) {
            Ephemeral = new EphemeralDonationCategoryContent();
            Ephemeral.Content(content);
        } else if (Type == DonationCategoryTypes.General) {
            General = new GeneralDonationCategoryContent();
            General.Content(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public DimensionDonationCategoryContent Dimension { get; private set; }
    public EphemeralDonationCategoryContent Ephemeral { get; private set; }
    public GeneralDonationCategoryContent General { get; private set; }
    
    public DonationCategoryType Type {
        get {
            if (Content().ContentType.Alias.EqualsInvariant(DimensionAlias)) {
                return DonationCategoryTypes.Dimension;
            } else if (Content().ContentType.Alias.EqualsInvariant(EphemeralAlias)) {
                return DonationCategoryTypes.Ephemeral;
            } else if (Content().ContentType.Alias.EqualsInvariant(GeneralAlias)) {
                return DonationCategoryTypes.General;
            } else {
                throw UnrecognisedValueException.For(Content().ContentType.Alias);
            }
        }
    }

    public IEnumerable<DonationCategoryContent> Categories {
        get {
            return Content().Children(x => x.IsDonationCategory())
                            .As<DonationCategoryContent>()
                            .ToList();
        }
    }
    
    public IEnumerable<DonationOptionContent> Options {
        get {
            return Content().Ancestor(ElementsConstants.Giving.Alias)
                            .As<GivingContent>()
                            .AllOptions
                            .Where(x => x.AllCategories.Any(y => y.Id == Id))
                            .ToList();
        }
    }
}