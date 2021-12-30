using N3O.Umbraco.Composing;
using N3O.Umbraco.Sponsorships.Database;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;

namespace N3O.Umbraco.Sponsorships {
    public class SponsorshipsComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Components().Append<DatabaseMigrationsComponent>();
        }
    
        public class DatabaseMigrationsComponent : IComponent {
            private readonly IRuntimeState _runtimeState;
            private readonly Lazy<IScopeProvider> _scopeProvider;
            private readonly Lazy<IMigrationPlanExecutor> _migrationPlanExecutor;
            private readonly Lazy<IKeyValueService> _keyValueService;

            public DatabaseMigrationsComponent(IRuntimeState runtimeState,
                                               Lazy<IScopeProvider> scopeProvider,
                                               Lazy<IMigrationPlanExecutor> migrationPlanExecutor,
                                               Lazy<IKeyValueService> keyValueService) {
                _runtimeState = runtimeState;
                _scopeProvider = scopeProvider;
                _migrationPlanExecutor = migrationPlanExecutor;
                _keyValueService = keyValueService;
            }

            public void Initialize() {
                if (_runtimeState.Level == RuntimeLevel.Run) {
                    var migrationPlan = new MigrationPlan(SponsorshipsConstants.Tables.Beneficiaries);
                    migrationPlan.From(string.Empty).To<BeneficiariesV1Migration>("v1");

                    var upgrader = new Upgrader(migrationPlan);
                    upgrader.Execute(_migrationPlanExecutor.Value, _scopeProvider.Value, _keyValueService.Value);
                }
            }

            public void Terminate() { }
        }
    }
}