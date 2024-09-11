using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Financial;
using System.Collections.Generic;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

public abstract class CrowdfunderContent<T> : UmbracoContent<T>
    where T : CrowdfunderContent<T> {
    public string AllocationsHash => GetValue(x => x.AllocationsHash);
    public CroppedImage BackgroundImage => GetValue(x => x.BackgroundImage);
    public HtmlEncodedString Body => GetValue(x => x.Body);
    public Currency Currency => GetValue(x => x.Currency);
    public string Description => GetValue(x => x.Description);
    public IEnumerable<GoalElement> Goals => GetNestedAs(x => x.Goals);
    public IEnumerable<HeroImagesElement> HeroImages => GetNestedAs(x => x.HeroImages);
    public string Title => GetValue(x => x.Title);
}