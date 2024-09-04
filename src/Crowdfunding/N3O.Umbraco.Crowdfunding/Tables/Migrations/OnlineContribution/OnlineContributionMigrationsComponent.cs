using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using static N3O.Umbraco.Crowdfunding.CrowdfundingConstants;

namespace N3O.Umbraco.Crowdfunding.Migrations;

public class OnlineContributionMigrationsComponent : IComponent {
    private readonly IRuntimeState _runtimeState;
    private readonly Lazy<ICoreScopeProvider> _scopeProvider;
    private readonly Lazy<IMigrationPlanExecutor> _migrationPlanExecutor;
    private readonly Lazy<IKeyValueService> _keyValueService;

    public OnlineContributionMigrationsComponent(IRuntimeState runtimeState,
                                                 Lazy<ICoreScopeProvider> scopeProvider,
                                                 Lazy<IMigrationPlanExecutor> migrationPlanExecutor,
                                                 Lazy<IKeyValueService> keyValueService) {
        _runtimeState = runtimeState;
        _scopeProvider = scopeProvider;
        _migrationPlanExecutor = migrationPlanExecutor;
        _keyValueService = keyValueService;
    }

    public void Initialize() {
        if (_runtimeState.Level == RuntimeLevel.Run) {
            var migrationPlan = new MigrationPlan(Tables.OnlineContributions.Name);
            migrationPlan.From(string.Empty).To<OnlineContributionMigration>("v1");

            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(_migrationPlanExecutor.Value, _scopeProvider.Value, _keyValueService.Value);
        }
    }

    public void Terminate() { }
}
