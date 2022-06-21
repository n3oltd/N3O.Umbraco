using N3O.Umbraco.Extensions;
using System;
using System.ComponentModel;

namespace N3O.Umbraco.Entities;

[TypeConverter(typeof(RevisionIdTypeConverter))]
public class RevisionId : IComparable<RevisionId>, IEquatable<RevisionId> {
    public EntityId Id { get; }
    public int Revision { get; }

    public RevisionId(string value) {
        var bits = value.Split(':');
        
        if (bits.Length != 2) {
            throw new ArgumentOutOfRangeException(nameof(value), value);
        }

        Id = new EntityId(bits[0]);
        Revision = int.Parse(bits[1]);
    }

    public RevisionId(EntityId id, int revision) {
        Id = id;
        Revision = revision;
    }

    public bool Equals(RevisionId other) {
        if (other == null) {
            return false;
        }

        return Id.Equals(other.Id) && Revision.Equals(other.Revision);
    }

    public int CompareTo(RevisionId other) {
        if (Id == other?.Id) {
            return Revision.CompareTo(other?.Revision);
        }

        return Id.CompareTo(other?.Id);
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
            return false;
        }

        if (ReferenceEquals(this, obj)) {
            return true;
        }

        return obj.GetType() == GetType() && Equals((RevisionId)obj);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Id.GetHashCode(), Revision.GetHashCode());
    }

    public override string ToString() {
        return $"{Id}:{Revision}";
    }

    public static bool IsValid(string value) {
        return TryParse(value) != null;
    }
    
    public bool RevisionMatches(int? revision) {
        return revision == Revision;
    }

    public static RevisionId TryParse(string value) {
        try {
            return new RevisionId(value);
        } catch {
            return null;
        }
    }

    public static bool operator ==(RevisionId lhs, RevisionId rhs) {
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

    public static bool operator !=(RevisionId lhs, RevisionId rhs) {
        return !(lhs == rhs);
    }

    public static implicit operator string(RevisionId revisionId) {
        return revisionId?.ToString();
    }

    public static implicit operator RevisionId(string revisionId) {
        if (!revisionId.HasValue()) {
            return null;
        }

        return new RevisionId(revisionId);
    }
}
