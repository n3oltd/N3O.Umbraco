using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Entities {
    public abstract class ChangeFeed<T> : IChangeFeed<T> where T : IEntity {
        private readonly ILogger _logger;

        protected ChangeFeed(ILogger logger) {
            _logger = logger;
        }

        public async Task ProcessChangeAsync(EntityChange entityChange, CancellationToken cancellationToken) {
            try {
                var typedEntityChange = new EntityChange<T>((T) entityChange.SessionEntity, (T) entityChange.DatabaseEntity, entityChange.Operation);

                await ProcessChangeAsync(typedEntityChange, cancellationToken);
            } catch (Exception ex) {
                _logger.LogError(ex,
                                 "Error processing change feed {ChangeFeedType} for {EntityType} with operation {Operation} and revision ID {RevisionId}",
                                 GetType().FullName.Quote(),
                                 typeof(T).GetFriendlyName(),
                                 entityChange.Operation,
                                 (entityChange.SessionEntity ?? entityChange.DatabaseEntity).RevisionId);
            }
        }

        protected abstract Task ProcessChangeAsync(EntityChange<T> entityChange, CancellationToken cancellationToken);
    }
}