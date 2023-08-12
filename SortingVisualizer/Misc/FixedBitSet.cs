using System.Collections;
using NotSupportedException = System.NotSupportedException;

namespace SortingVisualizer.Misc;

public class FixedBitSet : IList<bool>
{
    private uint[] _data;
    private long _size;

    /// <summary>
    /// Constructs a fixed bitset. The size must be a multiple of 32.
    /// </summary>
    /// <param name="size">the size.</param>
    public FixedBitSet(long size)
    {
        if ((size & 0x1F) != 0)
            throw new ArgumentException("Size is not a multiple of 32", nameof(size));
        _size = size;
        _data = new uint[_size / 32];
    }
    
    public IEnumerator<bool> GetEnumerator()
    {
        throw new NotImplementedException();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public void Add(bool item)
    {
        throw new NotSupportedException("FixedBitSet is a fixed-size container");
    }
    
    public void Clear()
    {
        throw new NotSupportedException("FixedBitSet is a fixed-size container");
    }
    
    public bool Contains(bool item)
    {
        int padding = (int) (_size & 0x1F);
        
        uint checkMask = item ? 0U : ~0U;
        for (long i = 0; i < _data.Length; i++)
        {
            if (_data[i] != checkMask)
                return true;
        }

        return false;
    }
    /// <summary>Copies the elements of the <see cref="FixedBitSet" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
    /// <remarks>
    /// It is advised not to copy between bit arrays and boolean arrays, as such operation is notoriously inefficient.
    /// </remarks>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="FixedBitSet" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="arrayIndex" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
    public void CopyTo(bool[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);
        if (checked(arrayIndex + _size) >= array.LongLength)
            throw new ArgumentException("Not enough space to copy data");
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex, 
                "Array index cannot be negative");

        for (long i = 0; i < _size; i++)
        {
            int shift = (int) (i & 0x1F);
            long index = i >> 5;

            array[arrayIndex + i] = ((_data[index] >> shift) & 1) != 0;
        }
    }
    
    public bool Remove(bool item)
    {
        throw new NotSupportedException("FixedBitSet is a fixed-size container");
    }

    public int Count => (int) _size;
    public bool IsReadOnly => false;
    public int IndexOf(bool item)
    {
        if (item)
        {
            long count = 0;
            for (long i = 0; i < _data.LongLength; i++)
            {
                count += uint.TrailingZeroCount(_data[i]);
                if (_data[i] != 0U)
                    return (int) count;
            }

        }
        else
        {
            long count = 0;
            for (long i = 0; i < _data.LongLength; i++)
            {
                count += uint.TrailingZeroCount(~_data[i]);
                if (_data[i] != ~0U)
                    return (int) count;
            }
        }
        return -1;
    }
    
    public void Insert(int index, bool item)
    {
        throw new NotSupportedException("FixedBitSet is a fixed-size container");
    }
    
    public void RemoveAt(int index)
    {
        throw new NotSupportedException("FixedBitSet is a fixed-size container");
    }
    
    public bool this[int index]
    {
        get => _data[index >> 5] >> (index & 0x1F) != 0;
        set
        {
            uint mask = 1U << (index & 0x1F);
            _data[index >> 5] = (_data[index >> 5] & ~mask) | (value ? mask : 0);
        }
    }
}