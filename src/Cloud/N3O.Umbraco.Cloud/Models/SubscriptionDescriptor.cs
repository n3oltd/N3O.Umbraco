using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class SubscriptionDescriptor : Value, IComparable<SubscriptionDescriptor>, IEquatable<SubscriptionDescriptor> {
    [JsonConstructor]
    public SubscriptionDescriptor(EntityId id, string name, string alias, string code, int number) {
        Id = id;
        Name = name;
        Alias = alias;
        Code = code;
        Number = number;
    }

    public SubscriptionDescriptor(EntityId id, string name = null, string alias = null)
        : this(id,
               name,
               alias,
               id.Value == Guid.Empty ? "0" : id.ToString().Substring(0, 8).TrimStart('0'),
               id.Value == Guid.Empty ? 0 : (int) Convert.ToInt64(id.ToString().Substring(0, 8).TrimStart('0'), 16)) { }

    public EntityId Id { get; }
    public string Name { get; }
    public string Alias { get; }
    public string Code { get; }
    public long Number { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
    }

    public bool Equals(SubscriptionDescriptor other) {
        if (other == null) {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public int CompareTo(SubscriptionDescriptor other) {
        return Id.CompareTo(other?.Id);
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
            return false;
        }

        if (ReferenceEquals(this, obj)) {
            return true;
        }

        return obj.GetType() == GetType() && Equals((SubscriptionDescriptor) obj);
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }

    public override string ToString() {
        return Id.ToString();
    }

    public static bool operator ==(SubscriptionDescriptor lhs, SubscriptionDescriptor rhs) {
        if (ReferenceEquals(lhs, rhs)) {
            return true;
        }

        if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) {
            return false;
        }

        return lhs.Equals(rhs);
    }

    public static bool operator !=(SubscriptionDescriptor lhs, SubscriptionDescriptor rhs) {
        return !(lhs == rhs);
    }

    public static implicit operator EntityId(SubscriptionDescriptor descriptor) {
        return descriptor?.Id;
    }

    public static implicit operator Guid?(SubscriptionDescriptor descriptor) {
        return descriptor?.Id;
    }

    public static implicit operator string(SubscriptionDescriptor descriptor) {
        return descriptor?.ToString();
    }

    public static implicit operator SubscriptionDescriptor(EntityId id) {
        return FromId(id);
    }

    public static SubscriptionDescriptor FromCode(string code) {
        if (code.HasValue()) {
            return new SubscriptionDescriptor($"{code.PadLeft(8, '0')}-0000-0000-0000-000000000000");
        } else {
            return null;
        }
    }

    public static SubscriptionDescriptor FromId(EntityId id) {
        if (id.HasValue()) {
            return new SubscriptionDescriptor(id);
        } else {
            return null;
        }
    }

    public static SubscriptionDescriptor FromNumber(long? number) {
        if (number.HasValue()) {
            return FromCode(number.GetValueOrThrow().ToString("x"));
        } else {
            return null;
        }
    }

    public static SubscriptionDescriptor Parse(string s) {
        return FromId(s) ?? FromCode(s) ?? FromNumber(long.TryParse(s, out var n) ? n : null);
    }
}
