using N3O.Umbraco.Cloud.Platforms.Commands;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Handlers;

public class CreateDefaultElementForCampaignHandler
    : IRequestHandler<CreateDefaultElementForCampaignCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly ICoreScopeProvider _coreScopeProvider;
    private readonly IContentEditor _contentEditor;
    private readonly DistributedCache _distributedCache;
    private readonly IBackgroundJob _backgroundJob;

    public CreateDefaultElementForCampaignHandler(IContentLocator contentLocator,
                                                  ICoreScopeProvider coreScopeProvider,
                                                  IContentEditor contentEditor,
                                                  DistributedCache distributedCache,
                                                  IBackgroundJob backgroundJob) {
        _contentLocator = contentLocator;
        _coreScopeProvider = coreScopeProvider;
        _contentEditor = contentEditor;
        _distributedCache = distributedCache;
        _backgroundJob = backgroundJob;
    }

    public Task<None> Handle(CreateDefaultElementForCampaignCommand req, CancellationToken cancellationToken) {
        var campaign = req.ContentId.Run(x => _contentLocator.ById<CampaignContent>(x), true);
        
        var formCreated = CreateDefaultElement(campaign, ElementTypes.DonationForm);
        var buttonCreated = CreateDefaultElement(campaign, ElementTypes.DonateButton);

        if (formCreated || buttonCreated) {
            // TODO Required for now as otherwise cache is not rebuilt
            _distributedCache.RefreshAllPublishedSnapshot();
            
            _backgroundJob.EnqueueCommand<PublishPlatformsContentCommand>();
        }
        
        return Task.FromResult(None.Empty);
    }

    private bool CreateDefaultElement<T>(CampaignContent content, T elementType) where T : ElementType {
        var exists = _contentLocator.All(elementType.ContentTypeAlias)
                                    .As<ElementContent>()
                                    .Where(x => x.Campaign?.Key == content.Key && x.IsSystemGenerated)
                                    .ToList();

        if (!exists.Any()) {
            var platformsElementsSettings = _contentLocator.Single<PlatformsElementsContent>();
        
            var contentPublisher = _contentEditor.New($"{content.Name} - {elementType.Name}",
                                                      platformsElementsSettings.Content().Key,
                                                      elementType.ContentTypeAlias);

            contentPublisher.Content.ContentPicker(AliasHelper<ElementContent>.PropertyAlias(x => x.Campaign)).SetContent(content);
            contentPublisher.Content.Boolean(AliasHelper<ElementContent>.PropertyAlias(x => x.IsSystemGenerated)).Set(true);

            if (elementType == ElementTypes.DonateButton) {
                contentPublisher.Content.Label(AliasHelper<DonateButtonElementContent>.PropertyAlias(x => x.Label)).Set("Donate");
            }
            

            // TODO have to do this because when notification handlers are raised, a separate umbraco scope is created
            // if any of the notification handlers have any service that access the database. Issue is resolved and
            // scheduled for next release https://github.com/umbraco/Umbraco-CMS/issues/18977
            using var scope = _coreScopeProvider.CreateCoreScope(autoComplete: true);

            using var _ = scope.Notifications.Suppress();
            
            contentPublisher.SaveAndPublish();
        
            scope.Complete();

            return true;
        }
        
        return false;
    }
}