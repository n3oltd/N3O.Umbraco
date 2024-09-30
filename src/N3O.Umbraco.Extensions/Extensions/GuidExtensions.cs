using System;

namespace N3O.Umbraco.Extensions;

public static class GuidExtensions {
    private static readonly int[] GuidByteOrder = { 15, 14, 13, 12, 11, 10, 9, 8, 6, 7, 4, 5, 0, 1, 2, 3 };
    
    public static Guid Decrement(this Guid guid) {
        var bytes = guid.ToByteArray();
        var carry = true;
    
        for (var i = 0; i < GuidByteOrder.Length && carry; i++) {
            var index = GuidByteOrder[i];
            var oldValue = bytes[index]--;
        
            carry = oldValue > bytes[index];
        }

        return new Guid(bytes);
    }

    public static Guid Increment(this Guid guid, int by) {
        if (by < 1) {
            throw new ArgumentOutOfRangeException(nameof(by), "Value cannot be less than 1");
        }

        var value = guid;
        for (var i = 0; i < by; i++) {
            value = Increment(value);
        }

        return value;
    }

    public static Guid Increment(this Guid guid) {
        var bytes = guid.ToByteArray();
        var carry = true;
    
        for (var i = 0; i < GuidByteOrder.Length && carry; i++) {
            var index = GuidByteOrder[i];
            var oldValue = bytes[index]++;
        
            carry = oldValue > bytes[index];
        }

        return new Guid(bytes);
    }
}
