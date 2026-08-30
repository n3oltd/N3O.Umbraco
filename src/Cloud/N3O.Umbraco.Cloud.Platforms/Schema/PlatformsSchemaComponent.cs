using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace N3O.Umbraco.Cloud.Platforms;

// Gated on the platforms site feature, which reaches the pod as Platforms:Enabled. The Key Vault provider
// reads once at startup, so turning the feature on takes a pod restart before this sees it
public class PlatformsSchemaComponent : IComponent {
    private readonly IConfiguration _configuration;
    private readonly IContentTypeService _contentTypeService;
    private readonly IDataTypeService _dataTypeService;
    private readonly Lazy<IPlatformsContentTypeSeeder> _contentTypeSeeder;
    private readonly Lazy<IPlatformsDataTypeSeeder> _dataTypeSeeder;
    private readonly Lazy<IKeyValueService> _keyValueService;
    private readonly Lazy<IMigrationPlanExecutor> _migrationPlanExecutor;
    private readonly Lazy<ICoreScopeProvider> _scopeProvider;
    private readonly Lazy<IPlatformsSchemaAudit> _schemaAudit;
    private readonly ILogger<PlatformsSchemaComponent> _logger;
    private readonly IRuntimeState _runtimeState;

    public PlatformsSchemaComponent(IConfiguration configuration,
                                    IContentTypeService contentTypeService,
                                    IDataTypeService dataTypeService,
                                    Lazy<IPlatformsContentTypeSeeder> contentTypeSeeder,
                                    Lazy<IPlatformsDataTypeSeeder> dataTypeSeeder,
                                    Lazy<IKeyValueService> keyValueService,
                                    Lazy<IMigrationPlanExecutor> migrationPlanExecutor,
                                    Lazy<ICoreScopeProvider> scopeProvider,
                                    Lazy<IPlatformsSchemaAudit> schemaAudit,
                                    ILogger<PlatformsSchemaComponent> logger,
                                    IRuntimeState runtimeState) {
        _configuration = configuration;
        _contentTypeService = contentTypeService;
        _dataTypeService = dataTypeService;
        _contentTypeSeeder = contentTypeSeeder;
        _dataTypeSeeder = dataTypeSeeder;
        _keyValueService = keyValueService;
        _migrationPlanExecutor = migrationPlanExecutor;
        _scopeProvider = scopeProvider;
        _schemaAudit = schemaAudit;
        _logger = logger;
        _runtimeState = runtimeState;
    }

    public void Initialize() {
        if (_runtimeState.Level != RuntimeLevel.Run || !IsEnabled()) {
            return;
        }

        // Creates only the types the site does not have. A type it already holds is never rewritten here
        _dataTypeSeeder.Value.Seed();
        _contentTypeSeeder.Value.Seed();

        // Changes to a type the site already holds arrive only as a migration step, which runs once
        if (CanMigrate()) {
            var upgrader = new Upgrader(new PlatformsSchemaPlan());

            upgrader.Execute(_migrationPlanExecutor.Value, _scopeProvider.Value, _keyValueService.Value);
        }

        // A site does not have to hold every platforms type, so a gap is reported rather than treated as a
        // failure; it is the only way to see which types a site is missing without opening its backoffice
        foreach (var gap in _schemaAudit.Value.FindGaps()) {
            _logger.LogInformation("Platforms schema gap: {Gap}", gap);
        }
    }

    public void Terminate() { }

    private bool CanMigrate() {
        return PlatformsSchemaPlan.RequiredContentTypes.All(x => _contentTypeService.Get(x) != null) &&
               PlatformsSchemaPlan.RequiredDataTypes.All(x => _dataTypeService.GetDataType(x) != null);
    }

    private bool IsEnabled() {
        var section = _configuration.GetSection(PlatformsSchemaConstants.ConfigurationSection);

        return section.GetValue<bool>(nameof(PlatformsFeatureSettings.Enabled));
    }
}
