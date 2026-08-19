using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class SubscriptionId : Value, IComparable<SubscriptionId>, IEquatable<SubscriptionId> {
    [JsonConstructor]
    public SubscriptionId(EntityId value, string name, string code) {
        Value = value;
        Name = name;
        Code = code;
    }

    public SubscriptionId(EntityId value, string name = null)
        : this(value,
               name,
               value.Value == Guid.Empty ? "0" : value.ToString().Substring(0, 8).TrimStart('0')) { }

    public EntityId Value { get; }
    public string Name { get; }
    public string Code { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Value;
    }

    public bool Equals(SubscriptionId other) {
        if (other == null) {
            return false;
        }

        return Value.Equals(other.Value);
    }

    public int CompareTo(SubscriptionId other) {
        return Value.CompareTo(other?.Value);
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
            return false;
        }

        if (ReferenceEquals(this, obj)) {
            return true;
        }

        return obj.GetType() == GetType() && Equals((SubscriptionId) obj);
    }

    public override int GetHashCode() {
        return Value.GetHashCode();
    }

    public override string ToString() {
        return Value.ToString();
    }

    public static bool operator ==(SubscriptionId lhs, SubscriptionId rhs) {
        if (ReferenceEquals(lhs, rhs)) {
            return true;
        }

        if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) {
            return false;
        }

        return lhs.Equals(rhs);
    }

    public static bool operator !=(SubscriptionId lhs, SubscriptionId rhs) {
        return !(lhs == rhs);
    }

    public static implicit operator EntityId(SubscriptionId subscriptionId) {
        return subscriptionId?.Value;
    }

    public static implicit operator Guid?(SubscriptionId subscriptionId) {
        return subscriptionId?.Value;
    }

    public static implicit operator string(SubscriptionId subscriptionId) {
        return subscriptionId?.ToString();
    }

    public static implicit operator SubscriptionId(EntityId id) {
        return FromId(id);
    }

    private static SubscriptionId FromCode(string code) {
        if (code.HasValue()) {
            return new SubscriptionId($"{code.PadLeft(8, '0')}-0000-0000-0000-000000000000");
        } else {
            return null;
        }
    }

    private static SubscriptionId FromId(EntityId id) {
        if (id.HasValue()) {
            return new SubscriptionId(id);
        } else {
            return null;
        }
    }

    public static SubscriptionId Parse(string s) {
        var subscriptionId = TryParse(s);

        if (s.HasValue() && subscriptionId == null) {
            throw new Exception($"{s} is not a valid subscription ID");
        }

        return subscriptionId;
    }

    public static SubscriptionId TryParse(string s) {
        if (!s.HasValue()) {
            return null;
        }

        return FromId(s) ?? FromCode(s);
    }
}
