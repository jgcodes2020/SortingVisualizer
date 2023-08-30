namespace SortingVisualizer.Sorting.Common;

/// <summary>
/// Heap sort. Works by constructing a binary heap in-place in the
/// array, then repeatedly extracting from it.
/// </summary>
public class HeapSort : SortingAlgorithm
{
    public HeapSort(BufferSet buffers) : base(buffers)
    {
    }

    protected override void DoSorting()
    {
        for (int i = Data.Length / 2 - 1; i >= 0; i--)
        {
            SiftDown(i, Data.Length);
        }

        for (int i = Data.Length - 1; i > 0; i--)
        {
            (Data[0], Data[i]) = (Data[i], Data[0]);
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

            if (left < heapLimit && Data[left] > Data[max]) max = left;
            if (right < heapLimit && Data[right] > Data[max]) max = right;

            if (max == i) 
                return;

            (Data[max], Data[i]) = (Data[i], Data[max]);
            i = max;
        }
    }

    private void FillHeapColours(int heapLimit)
    {
        for (int i = 0; i < Data.Length; i++)
        {
            if (i >= heapLimit)
                Palette[i] = 0xFF_FFFFFF;
            else
            {
                Palette[i] = ((32 - int.LeadingZeroCount(i)) % 3) switch
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
        Palette[pos] = 0xFF_CC0000;
        Palette[startPos] = 0xFF_0066CC;
        if (left >= 0 && left < heapLimit)
            Palette[left] = 0xFF_00CC00;
        if (right >= 0 && right < heapLimit)
            Palette[right] = 0xFF_00CC00;
        
        SyncPoint();
    }
    private void SyncPointSwap(int heapLimit, int a, int b)
    {
        FillHeapColours(heapLimit);
        Palette[a] = 0xFF_CC0000;
        Palette[b] = 0xFF_CC0000;
        
        SyncPoint();
    }
}