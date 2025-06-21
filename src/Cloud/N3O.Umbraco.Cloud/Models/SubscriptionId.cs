using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class SubscriptionId : Value, IComparable<SubscriptionId>, IEquatable<SubscriptionId> {
    [JsonConstructor]
    public SubscriptionId(string code, int number, EntityId value) {
        Code = code;
        Number = number;
        Value = value;
    }

    public SubscriptionId(EntityId entityId) {
        Value = entityId;
        
        if (entityId.Value == Guid.Empty) {
            Code = "0";
            Number = 0;
        } else {
            Code = entityId.ToString().Substring(0, 8).TrimStart('0');
            Number = Convert.ToInt64(Code, 16);
        }
    }
    
    public string Code { get; }
    public long Number { get; }
    public EntityId Value { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Code;
        yield return Number;
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

        if (ReferenceEquals(lhs, null)) {
            return false;
        }

        if (ReferenceEquals(rhs, null)) {
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
        return new SubscriptionId(id);
    }
    
    public static SubscriptionId FromCode(string code) {
        if (code.HasValue()) {
            return new SubscriptionId($"{code.PadLeft(8, '0')}-0000-0000-0000-000000000000");
        } else {
            return null;
        }
    }

    public static SubscriptionId FromId(EntityId id) {
        if (id.HasValue()) {
            return new(id);
        } else {
            return null;
        }
    }
    
    public static SubscriptionId FromNumber(long? number) {
        if (number.HasValue()) {
            var code = number.GetValueOrThrow().ToString("x");

            return FromCode(code);
        } else {
            return null;
        }
    }
}