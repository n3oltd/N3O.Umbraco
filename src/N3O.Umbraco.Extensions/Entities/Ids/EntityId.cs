using N3O.Umbraco.Extensions;
using System;
using System.ComponentModel;

namespace N3O.Umbraco.Entities;

[TypeConverter(typeof(EntityIdTypeConverter))]
public class EntityId : IComparable<EntityId>, IEquatable<EntityId> {
    public EntityId(Guid value) {
        Value = value;
    }

    public EntityId(string value) : this(Guid.Parse(value)) { }

    public Guid Value { get; }

    public bool Equals(EntityId other) {
        if (other == null) {
            return false;
        }

        return Value.Equals(other.Value);
    }

    public int CompareTo(EntityId other) {
        return Value.CompareTo(other?.Value);
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
            return false;
        }

        if (ReferenceEquals(this, obj)) {
            return true;
        }

        return obj.GetType() == GetType() && Equals((EntityId)obj);
    }

    public override int GetHashCode() {
        return Value.GetHashCode();
    }

    public override string ToString() {
        return Value.ToString();
    }

    public static EntityId TryParse(string value) {
        if (value?.Contains(":") == true) {
            value = value.Substring(0, value.IndexOf(':'));
        }
        
        if (Guid.TryParse(value, out var guid)) {
            return new EntityId(guid);
        }

        return null;
    }

    public static bool operator ==(EntityId lhs, EntityId rhs) {
        if (ReferenceEquals(lhs, rhs)) {
            return true;
        }

        if (ReferenceEquals(lhs, null)) {
            return false;
        }

        if (ReferenceEquals(rhs, null)) {
            return false;
        }

        return lhs.Equals(rhs);
    }

    public static bool operator !=(EntityId lhs, EntityId rhs) {
        return !(lhs == rhs);
    }
    
    public static EntityId New() => new(Guid.NewGuid());

    public static implicit operator Guid(EntityId id) {
        return id.Value;
    }

    public static implicit operator Guid?(EntityId id) {
        return id?.Value;
    }

    public static implicit operator string(EntityId id) {
        return id?.ToString();
    }

    public static implicit operator EntityId(string id) {
        if (!id.HasValue()) {
            return null;
        }

        return new EntityId(id);
    }

    public static implicit operator EntityId(Guid id) {
        return new EntityId(id);
    }
}
