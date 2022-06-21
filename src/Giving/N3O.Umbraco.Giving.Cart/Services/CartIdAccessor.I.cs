using N3O.Umbraco.Entities;

namespace N3O.Umbraco.Giving.Cart;

public interface ICartIdAccessor {
    EntityId GetId();
    RevisionId GetRevisionId();
}
