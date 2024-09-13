using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Financial;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Content;

public interface ICrowdfunderContent {
    public Guid Key { get; }
    public CroppedImage BackgroundImage { get; }
    public HtmlEncodedString Body { get; }
    public Currency Currency { get; }
    public string Description { get; }
    public IEnumerable<GoalElement> Goals { get; }
    public IEnumerable<HeroImagesElement> HeroImages { get; }
    public string Title { get; }
}