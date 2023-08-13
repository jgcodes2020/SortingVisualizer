namespace SortingVisualizer.Sorting.Common;

public class HeapSort : SortingAlgorithm
{
    public HeapSort(int length) : base(length)
    {
    }

    protected override void DoSorting()
    {
        for (int i = _data.Length / 2 - 1; i >= 0; i--)
        {
            SiftDown(i, _data.Length);
        }

        for (int i = _data.Length - 1; i > 0; i--)
        {
            (_data[0], _data[i]) = (_data[i], _data[0]);
            SyncPointSwap(i, 0, i);
            SiftDown(0, i);
        }
    }

    private void SiftDown(int pos, int heapLimit)
    {
        int i = pos;
        while (true)
        {
            int max = i;
            int left = i * 2, right = left + 1;
            SyncPointSift(heapLimit, pos, i, left, right);

            if (left < heapLimit && _data[left] > _data[max]) max = left;
            if (right < heapLimit && _data[right] > _data[max]) max = right;

            if (max == i) 
                return;

            (_data[max], _data[i]) = (_data[i], _data[max]);
            i = max;
        }
    }

    private void FillHeapColours(int heapLimit)
    {
        for (int i = 0; i < Data.Length; i++)
        {
            if (i >= heapLimit)
                _palette[i] = 0xFF_FFFFFF;
            else
            {
                _palette[i] = ((32 - int.LeadingZeroCount(i)) % 3) switch
                {
                    0 => 0xFF_FFFF80,
                    1 => 0xFF_80FFFF,
                    2 => 0xFF_FF80FF,
                    _ => 0xFF_FFFFFF
                };
            }
        }
    }

    private void SyncPointSift(int heapLimit, int startPos, int pos, int left, int right)
    {
        FillHeapColours(heapLimit);
        _palette[pos] = 0xFF_CC0000;
        _palette[startPos] = 0xFF_0066CC;
        if (left >= 0 && left < heapLimit)
            _palette[left] = 0xFF_00CC00;
        if (right >= 0 && right < heapLimit)
            _palette[right] = 0xFF_00CC00;
        
        SyncPoint();
    }
    private void SyncPointSwap(int heapLimit, int a, int b)
    {
        FillHeapColours(heapLimit);
        _palette[a] = 0xFF_CC0000;
        _palette[b] = 0xFF_CC0000;
        
        SyncPoint();
    }
}