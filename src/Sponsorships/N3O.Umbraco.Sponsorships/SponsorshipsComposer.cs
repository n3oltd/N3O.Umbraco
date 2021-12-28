using N3O.Umbraco.Composing;
using N3O.Umbraco.Sponsorships.Database;
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
            private readonly IScopeProvider _scopeProvider;
            private readonly IMigrationPlanExecutor _migrationPlanExecutor;
            private readonly IKeyValueService _keyValueService;

            public DatabaseMigrationsComponent(IScopeProvider scopeProvider,
                                               IMigrationPlanExecutor migrationPlanExecutor,
                                               IKeyValueService keyValueService) {
                _scopeProvider = scopeProvider;
                _migrationPlanExecutor = migrationPlanExecutor;
                _keyValueService = keyValueService;
            }

            public void Initialize() {
                var migrationPlan = new MigrationPlan(SponsorshipsConstants.Tables.Beneficiaries);
                migrationPlan.From(string.Empty).To<BeneficiariesV1Migration>("v1");

                var upgrader = new Upgrader(migrationPlan);
                upgrader.Execute(_migrationPlanExecutor, _scopeProvider, _keyValueService);
            }

            public void Terminate() { }
        }
    }
}