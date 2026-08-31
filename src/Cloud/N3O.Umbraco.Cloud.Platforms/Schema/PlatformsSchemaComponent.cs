using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
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
        var blockers = FindMigrationBlockers().ToList();

        if (blockers.None()) {
            var upgrader = new Upgrader(new PlatformsSchemaPlan());

            upgrader.Execute(_migrationPlanExecutor.Value, _scopeProvider.Value, _keyValueService.Value);
        } else {
            // Blocked is not a state the site recovers from on its own, so it is reported as a warning. A
            // step that ran against a missing type would fail, and a failed step leaves the scope unusable
            // for the rest of the boot, which takes the uSync import down with it
            _logger.LogWarning("Platforms schema migrations blocked, waiting on {Blockers}",
                               string.Join(", ", blockers));
        }

        // A site does not have to hold every platforms type, so a gap is reported rather than treated as a
        // failure; it is the only way to see which types a site is missing without opening its backoffice
        foreach (var gap in _schemaAudit.Value.FindGaps()) {
            _logger.LogInformation("Platforms schema gap: {Gap}", gap);
        }
    }

    public void Terminate() { }

    // A type is looked up the way its designer would find it, by deterministic key as well as by the alias
    // or name, so a site that renamed one is not read as not having it and blocked from every step forever
    private IEnumerable<string> FindMigrationBlockers() {
        foreach (var alias in PlatformsSchemaPlan.RequiredContentTypes) {
            var contentType = _contentTypeService.Get(UmbracoId.Deterministic(IdScope.ContentType, alias)) ??
                              _contentTypeService.Get(alias);

            if (contentType == null) {
                yield return alias;
            } else if (contentType.IsElement) {
                // Every step builds these as document types, so one the site holds as an element would have
                // the step refuse it. Reporting it here keeps that out of the plan, where a step that threw
                // would leave the scope unusable for the rest of the boot
                yield return $"{alias} (held as an element type)";
            }
        }

        foreach (var name in PlatformsSchemaPlan.RequiredDataTypes) {
            if (_dataTypeService.GetDataType(UmbracoId.Deterministic(IdScope.DataType, name)) == null &&
                _dataTypeService.GetDataType(name) == null) {
                yield return name;
            }
        }
    }

    private bool IsEnabled() {
        var section = _configuration.GetSection(PlatformsSchemaConstants.ConfigurationSection);

        return section.GetValue<bool>(nameof(PlatformsFeatureSettings.Enabled));
    }
}
