﻿using N3O.Umbraco.Content;
using N3O.Umbraco.Crm;
using N3O.Umbraco.Cropper.Extensions;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public partial class CreateOrUpdateFundraiserHandlers {
    private readonly IContentLocator _contentLocator;
    private readonly IContentEditor _contentEditor;
    private readonly IMemberManager _memberManager;
    private readonly IAccountIdentityAccessor _accountIdentityAccessor;

    public CreateOrUpdateFundraiserHandlers(IContentLocator contentLocator,
                                            IContentEditor contentEditor,
                                            IMemberManager memberManager,
                                            IAccountIdentityAccessor accountIdentityAccessor) {
        _contentLocator = contentLocator;
        _contentEditor = contentEditor;
        _memberManager = memberManager;
        _accountIdentityAccessor = accountIdentityAccessor;
    }
    
    private async Task PopulateFundraiserAsync(IContentPublisher contentPublisher, CreateFundraiserReq req) {
        var member = await _memberManager.GetCurrentPublishedMemberAsync();
        var accountReference = _accountIdentityAccessor.GetReference();

        if (!accountReference.HasValue()) {
            throw new Exception("Could not resolve a valid account reference when populating fundraiser");
        }
        
        var campaign = _contentLocator.ById<CampaignContent>(req.CampaignId.GetValueOrThrow());
        

        contentPublisher.Content.Label(CrowdfundingConstants.Crowdfunder.Properties.Name).Set(req.Name);
        contentPublisher.Content.DataList(CrowdfundingConstants.Crowdfunder.Properties.Currency).SetLookups(req.Currency);
        
        contentPublisher.Content.Label(CrowdfundingConstants.Fundraiser.Properties.Slug).Set(req.Slug);
        contentPublisher.Content.Label(CrowdfundingConstants.Fundraiser.Properties.AccountReference).Set(accountReference);
        contentPublisher.Content.ContentPicker(CrowdfundingConstants.Fundraiser.Properties.Campaign).SetContent(campaign);
        contentPublisher.Content.Label(CrowdfundingConstants.Fundraiser.Properties.Status).Set(FundraiserStatuses.Pending.Name);
        contentPublisher.Content.ContentPicker(CrowdfundingConstants.Fundraiser.Properties.Owner).SetMember(member);
        
        PopulateDefaultContent(contentPublisher, campaign);
        PopulateFundraiserGoals(contentPublisher, req.Goals.Items, campaign);
    }
    
    private void PopulateFundraiserGoals(IContentPublisher contentPublisher,
                                         IEnumerable<FundraiserGoalReq> reqs,
                                         CampaignContent campaign) {
        var nestedContent = contentPublisher.Content.Nested(CrowdfundingConstants.Crowdfunder.Properties.Goals);
        
        foreach (var req in reqs) {
            var campaignGoal = campaign.GoalOptions.SingleOrDefault(x => x.GoalId == req.GoalId.GetValueOrThrow());

            if (campaignGoal == null) {
                throw new Exception($"No campaign goal found with ID {req.GoalId}");
            }
            
            if (campaignGoal.Type == AllocationTypes.Fund) {
                AddFundraiserFundGoal(nestedContent, req, campaignGoal);
            } else if (campaignGoal.Type == AllocationTypes.Feedback) {
                AddFundraiserFeedbackGoal(nestedContent, req, campaignGoal);
            } else {
                throw UnrecognisedValueException.For(campaignGoal.Type);
            }
        }
    }
    
    private void AddFundraiserFundGoal(NestedPropertyBuilder nestedPropertyBuilder,
                                       FundraiserGoalReq req,
                                       CampaignGoalOptionElement goalOption) {
        var contentBuilder = nestedPropertyBuilder.Add(CrowdfundingConstants.Goal.Fund.Alias, goalOption.GoalId.Increment());
        
        PopulateFundraiserGoal(contentBuilder, req, goalOption);
        
        contentBuilder.ContentPicker(CrowdfundingConstants.Goal.Fund.Properties.DonationItem)
                      .SetContent(goalOption.Fund.DonationItem);
    }
    
    private void AddFundraiserFeedbackGoal(NestedPropertyBuilder nestedPropertyBuilder,
                                           FundraiserGoalReq req,
                                           CampaignGoalOptionElement goalOption) {
        var contentBuilder = nestedPropertyBuilder.Add(CrowdfundingConstants.Goal.Feedback.Alias, goalOption.GoalId);
        
        PopulateFundraiserGoal(contentBuilder, req, goalOption);
        
        contentBuilder.ContentPicker(CrowdfundingConstants.Goal.Feedback.Properties.Scheme).SetContent(goalOption.Feedback.Scheme);

        PopulateCustomFields(contentBuilder,
                             goalOption.Feedback.Scheme,
                             req.Feedback.OrEmpty(x => x?.CustomFields.Entries));
    }
    
    private void CopyCroppedImage(IContentBuilder builder, CroppedImage image, string destinationPropertyAlias) {
        var sourceImage = image.GetUncroppedImage();

        var cropper = builder.Cropper(destinationPropertyAlias).SetImage(sourceImage);

        foreach (var crop in sourceImage.Crops) {
            cropper.AddCrop().CropTo(crop.X, crop.Y, crop.Width, crop.Height);
        }
    }
    
    private void PopulateFundraiserGoal(IContentBuilder contentBuilder,
                                        FundraiserGoalReq req,
                                        CampaignGoalOptionElement goalOption) {
        contentBuilder.Numeric(CrowdfundingConstants.Goal.Properties.Amount).SetDecimal(req.Amount);
        contentBuilder.TextBox(CrowdfundingConstants.Goal.Properties.Name).Set(goalOption.Name);
        contentBuilder.ContentPicker(CrowdfundingConstants.Goal.Properties.FundDimension1).SetContent(req.FundDimensions.Dimension1);
        contentBuilder.ContentPicker(CrowdfundingConstants.Goal.Properties.FundDimension2).SetContent(req.FundDimensions.Dimension2);
        contentBuilder.ContentPicker(CrowdfundingConstants.Goal.Properties.FundDimension3).SetContent(req.FundDimensions.Dimension3);
        contentBuilder.ContentPicker(CrowdfundingConstants.Goal.Properties.FundDimension4).SetContent(req.FundDimensions.Dimension4);
        contentBuilder.ContentPicker(CrowdfundingConstants.Goal.Properties.Tags).SetContent(goalOption.Tags);
        
        var priceHandlesBuilder = contentBuilder.Nested(CrowdfundingConstants.Goal.Properties.PriceHandles);

        foreach (var priceHandle in goalOption.PriceHandles.OrEmpty()) {
            AddPriceHandle(priceHandlesBuilder, priceHandle);
        }
    }
    
    private void PopulateDefaultContent(IContentPublisher contentPublisher, CampaignContent campaign) {
        contentPublisher.Content.TextBox(CrowdfundingConstants.Crowdfunder.Properties.Description).Set(campaign.Description);
        contentPublisher.Content.Raw(CrowdfundingConstants.Crowdfunder.Properties.Body).Set(campaign.Body);
        
        CopyCroppedImage(contentPublisher.Content, campaign.BackgroundImage, CrowdfundingConstants.Crowdfunder.Properties.BackgroundImage);
        PopulateHeroImages(contentPublisher, campaign);
    }
    
    private void PopulateHeroImages(IContentPublisher contentPublisher, CampaignContent campaign) {
        var nestedContent = contentPublisher.Content.Nested(CrowdfundingConstants.Crowdfunder.Properties.HeroImages);
            
        foreach (var heroImage in campaign.HeroImages) {
            CopyCroppedImage(nestedContent.Add(CrowdfundingConstants.HeroImages.Alias),
                             heroImage.Image,
                             CrowdfundingConstants.HeroImages.Properties.Image);
        }
    }
    
    private void PopulateCustomFields(IContentBuilder contentBuilder,
                                      FeedbackScheme feedbackScheme,
                                      IEnumerable<FeedbackNewCustomFieldReq> newCustomFields) {
        var nestedContent = contentBuilder.Nested(CrowdfundingConstants.Goal.Feedback.Properties.CustomFields);

        foreach (var newCustomField in newCustomFields) {
            var customFieldBuilder = nestedContent.Add(GivingConstants.Aliases.FeedbackCustomField.ContentType);
            var feedbackCustomField = newCustomField.ToFeedbackCustomField(feedbackScheme);
            
            customFieldBuilder.TextBox(GivingConstants.Aliases.FeedbackCustomField.Properties.Alias).Set(newCustomField.Alias);
            customFieldBuilder.DataList(GivingConstants.Aliases.FeedbackCustomField.Properties.Type).SetLookups(feedbackCustomField.Type);
            customFieldBuilder.TextBox(GivingConstants.Aliases.FeedbackCustomField.Properties.Name).Set(feedbackCustomField.Name);

            if (feedbackCustomField.Type == FeedbackCustomFieldTypes.Bool) {
                customFieldBuilder.Boolean(GivingConstants.Aliases.FeedbackCustomField.Properties.Bool).Set(feedbackCustomField.Bool);
            } else if (feedbackCustomField.Type == FeedbackCustomFieldTypes.Date) {
                customFieldBuilder.DateTime(GivingConstants.Aliases.FeedbackCustomField.Properties.Date).SetDate(feedbackCustomField.Date);
            }  else if (feedbackCustomField.Type == FeedbackCustomFieldTypes.Text) {
                customFieldBuilder.TextBox(GivingConstants.Aliases.FeedbackCustomField.Properties.Text).Set(feedbackCustomField.Text);
            } else {
                throw UnrecognisedValueException.For(feedbackCustomField.Type);
            }
        }
    }
    
    private void AddPriceHandle(NestedPropertyBuilder propertyBuilder, PriceHandleElement priceHandleElement) {
        var contentBuilder = propertyBuilder.Add(GivingConstants.Aliases.PriceHandle.ContentType);
        
        contentBuilder.Numeric(GivingConstants.Aliases.PriceHandle.Properties.Amount)
                      .SetDecimal(priceHandleElement.Amount);
        
        contentBuilder.TextBox(GivingConstants.Aliases.PriceHandle.Properties.Description)
                      .Set(priceHandleElement.Description);
    }

    private ValidationException ToValidationException(PublishResult publishResult) {
        var error = string.Join("\n", publishResult.EventMessages
                                                   .OrEmpty(x => x.GetAll())
                                                   .Select(x => $"{x.MessageType}: {x.Message}"));
            
        return new ValidationException(ValidationFailure.WithMessage(null, error));
    }
}