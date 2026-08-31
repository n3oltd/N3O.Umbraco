using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.DataTypes;
using N3O.Umbraco.Extensions;
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
    private readonly IContentTypeEditor _contentTypeEditor;
    private readonly Lazy<IPlatformsContentTypeSeeder> _contentTypeSeeder;
    private readonly IDataTypeEditor _dataTypeEditor;
    private readonly Lazy<IPlatformsDataTypeSeeder> _dataTypeSeeder;
    private readonly Lazy<IKeyValueService> _keyValueService;
    private readonly ILogger<PlatformsSchemaComponent> _logger;
    private readonly Lazy<IMigrationPlanExecutor> _migrationPlanExecutor;
    private readonly IRuntimeState _runtimeState;
    private readonly Lazy<IPlatformsSchemaAudit> _schemaAudit;
    private readonly Lazy<ICoreScopeProvider> _scopeProvider;

    public PlatformsSchemaComponent(IConfiguration configuration,
                                    IContentTypeEditor contentTypeEditor,
                                    Lazy<IPlatformsContentTypeSeeder> contentTypeSeeder,
                                    IDataTypeEditor dataTypeEditor,
                                    Lazy<IPlatformsDataTypeSeeder> dataTypeSeeder,
                                    Lazy<IKeyValueService> keyValueService,
                                    ILogger<PlatformsSchemaComponent> logger,
                                    Lazy<IMigrationPlanExecutor> migrationPlanExecutor,
                                    IRuntimeState runtimeState,
                                    Lazy<IPlatformsSchemaAudit> schemaAudit,
                                    Lazy<ICoreScopeProvider> scopeProvider) {
        _configuration = configuration;
        _contentTypeEditor = contentTypeEditor;
        _contentTypeSeeder = contentTypeSeeder;
        _dataTypeEditor = dataTypeEditor;
        _dataTypeSeeder = dataTypeSeeder;
        _keyValueService = keyValueService;
        _logger = logger;
        _migrationPlanExecutor = migrationPlanExecutor;
        _runtimeState = runtimeState;
        _schemaAudit = schemaAudit;
        _scopeProvider = scopeProvider;
    }

    public void Initialize() {
        if (_runtimeState.Level != RuntimeLevel.Run || !IsEnabled()) {
            return;
        }

        // Creates only the types the site does not have. A type it already holds is never rewritten here
        _dataTypeSeeder.Value.Seed();
        _contentTypeSeeder.Value.Seed();

        // Changes to a type the site already holds arrive only as a migration step, which runs once
        var blockers = FindMigrationBlockers();

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
    private IReadOnlyList<string> FindMigrationBlockers() {
        var blockers = new List<string>();

        foreach (var alias in PlatformsSchemaPlan.RequiredContentTypes) {
            var contentType = _contentTypeEditor.Find(alias);

            if (contentType == null) {
                blockers.Add(alias);
            } else if (contentType.IsElement) {
                // Every step builds these as document types, so one the site holds as an element would have
                // the step refuse it. Reporting it here keeps that out of the plan, where a step that threw
                // would leave the scope unusable for the rest of the boot
                blockers.Add($"{alias} (held as an element type)");
            }
        }

        blockers.AddRange(PlatformsSchemaPlan.RequiredDataTypes.Where(x => _dataTypeEditor.Find(x) == null));

        return blockers;
    }

    private bool IsEnabled() {
        var section = _configuration.GetSection(PlatformsSchemaConstants.ConfigurationSection);

        return section.GetValue<bool>(nameof(PlatformsFeatureSettings.Enabled));
    }
}
