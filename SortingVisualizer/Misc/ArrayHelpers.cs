using System.Numerics;
using System.Security.Cryptography;
using Pcg;

namespace SortingVisualizer.Misc;

public static class ArrayHelpers
{
    private static Pcg32 _rng;

    static ArrayHelpers()
    {
        byte[] seedBytes = new byte[16];
        using (RandomNumberGenerator secureRNG = RandomNumberGenerator.Create())
        {
            secureRNG.GetBytes(seedBytes);
        }

        ulong seedState = BitConverter.ToUInt64(seedBytes);
        ulong seedSeq = BitConverter.ToUInt64(seedBytes, 8);
        _rng = new Pcg32(seedState, seedSeq);
    }

    private static long GeneratePositive(long boundL)
    {
        if (boundL <= 0)
            throw new ArgumentException("Maximum must be > 1", nameof(boundL));
        if (boundL == 1)
            return 0;
        if (boundL <= (long) uint.MaxValue + 1)
            return _rng.GenerateNext((uint) boundL);

        ulong boundUL = (ulong) boundL;
        ulong maxUL = unchecked(0 - boundUL) % boundUL;
        while (true)
        {
            ulong value = ((ulong) _rng.GenerateNext() << 32) | _rng.GenerateNext();
            if (value > maxUL)
                return (long) (value % boundUL);
        }
        
    }

    public static void Shuffle<T>(T[] array)
    {
        if (array.LongLength <= 1) 
            return;

        for (long i = array.LongLength; i > 1;)
        {
            long n = GeneratePositive(i--);
            (array[i], array[n]) = (array[n], array[i]);
        }
    }

    public static void FillRange<T>(T[] array, T first, T step) where T : IBinaryInteger<T>
    {
        T p = first;
        for (long i = 0; i < array.LongLength; i++)
        {
            array[i] = p;
            p += step;
        }
    }

    public static void Shuffle<T>(this Span<T> data)
    {
        if (data.IsEmpty)
            throw new ArgumentException("Span was empty!", nameof(data));
        if (data.Length <= 1)
            return;
        for (int i = data.Length; i > 1;)
        {
            int n = (int) _rng.GenerateNext((uint) i--);
            (data[i], data[n]) = (data[n], data[i]);
        }
    }

    public static void FillRange<T>(this Span<T> data, T first, T step) where T : IBinaryInteger<T>
    {
        if (data.IsEmpty)
            throw new ArgumentException("Span was empty!", nameof(data));
        T p = first;
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = p;
            p += step;
        }
    }
}