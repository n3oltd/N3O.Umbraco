using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Cart.Database;
using N3O.Umbraco.Giving.Cart.Hosting;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Giving.Cart {
    public class CartComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<ICartAccessor, CartAccessor>();
            builder.Services.AddScoped<CartCookieMiddleware, CartCookieMiddleware>();
            builder.Services.AddTransient<ICartIdAccessor, CartIdAccessor>();
            builder.Services.AddTransient<ICartRepository, CartRepository>();
            builder.Services.AddTransient<ICartValidator, CartValidator>();

            builder.Services.AddOpenApiDocument(CartConstants.ApiName);

            builder.Services.Configure<UmbracoPipelineOptions>(options => {
                var filter = new UmbracoPipelineFilter("Cart");
                filter.PostPipeline = app => app.UseMiddleware<CartCookieMiddleware>();
            
                options.AddFilter(filter);
            });
            
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
                    var migrationPlan = new MigrationPlan(CartConstants.Tables.Carts);
                    migrationPlan.From(string.Empty).To<CartsV1Migration>("v1");

                    var upgrader = new Upgrader(migrationPlan);
                    upgrader.Execute(_migrationPlanExecutor.Value, _scopeProvider.Value, _keyValueService.Value);
                }
            }

            public void Terminate() { }
        }
    }
}
