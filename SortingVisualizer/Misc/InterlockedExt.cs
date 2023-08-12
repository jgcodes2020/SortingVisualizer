using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SortingVisualizer.Misc;

public static class InterlockedExt
{
    public static TEnum Or<TEnum>(ref TEnum x, TEnum y) where TEnum: struct, Enum
    {
        return Unsafe.SizeOf<TEnum>() switch
        {
            4 => Or32(ref x, y),
            8 => Or64(ref x, y),
            _ => throw new ArgumentException("Type must be 4 or 8 bytes long")
        };

        TEnum Or32(ref TEnum a, TEnum b)
        {
            ref uint src = ref Unsafe.As<TEnum, uint>(ref a);
            uint value = Unsafe.As<TEnum, uint>(ref b);
            uint res = Interlocked.Or(ref src, value);
            return Unsafe.As<uint, TEnum>(ref res);
        }
        TEnum Or64(ref TEnum a, TEnum b)
        {
            ref ulong src = ref Unsafe.As<TEnum, ulong>(ref a);
            ulong value = Unsafe.As<TEnum, ulong>(ref b);
            ulong res = Interlocked.Or(ref src, value);
            return Unsafe.As<ulong, TEnum>(ref res);
        }
    }
    public static TEnum And<TEnum>(ref TEnum x, TEnum y) where TEnum: struct, Enum
    {
        return Unsafe.SizeOf<TEnum>() switch
        {
            4 => And32(ref x, y),
            8 => And64(ref x, y),
            _ => throw new ArgumentException("Type must be 4 or 8 bytes long")
        };

        TEnum And32(ref TEnum a, TEnum b)
        {
            ref uint src = ref Unsafe.As<TEnum, uint>(ref a);
            uint value = Unsafe.As<TEnum, uint>(ref b);
            uint res = Interlocked.And(ref src, value);
            return Unsafe.As<uint, TEnum>(ref res);
        }
        TEnum And64(ref TEnum a, TEnum b)
        {
            ref ulong src = ref Unsafe.As<TEnum, ulong>(ref a);
            ulong value = Unsafe.As<TEnum, ulong>(ref b);
            ulong res = Interlocked.And(ref src, value);
            return Unsafe.As<ulong, TEnum>(ref res);
        }
    }
}