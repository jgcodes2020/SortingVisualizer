namespace SortingVisualizer.Sorting.Hybrid;

/// <summary>
/// Introsort. Uses quicksort for the most part, but switches
/// to insertion sort for small arrays, and heapsort if the
/// quicksort goes horribly.
/// </summary>
public class Introsort : SortingAlgorithm
{
    public Introsort(BufferSet buffers) : base(buffers)
    {
    }

    protected override void DoSorting()
    {
        int maxDepth = 2 * (32 - int.Log2(Data.Length));
        DoSorting(0, Data.Length, maxDepth);
    }

    private void DoSorting(int begin, int end, int depthRemain)
    {
        if (end - begin < 16)
        {
            InsertionSort(begin, end);
            return;
        }

        if (depthRemain == 0)
        {
            HeapSort(begin, end);
            return;
        }
        
        int curtain = Partition(begin, end);
        DoSorting(begin, curtain, depthRemain - 1);
        DoSorting(curtain, end, depthRemain - 1);
    }

    private int Partition(int begin, int end)
    {
        end -= 1;

        // Median-of-3 pivot selection
        if (end - begin >= 5)
        {
            int mid = (begin + end) / 2;

            PivotSyncPoint(begin, end, mid);
            if (Data[mid] > Data[begin] != Data[mid] > Data[end])
                (Data[begin], Data[mid]) = (Data[mid], Data[begin]);
            else
            {
                PivotSyncPoint(begin, end, end);
                if (Data[end] > Data[mid] != Data[end] > Data[begin])
                    (Data[begin], Data[end]) = (Data[end], Data[begin]);
            }

            PivotSyncPoint(begin, end, begin);
        }

        // Partitioning
        {
            uint pivot = Data[begin];
            int i = begin, j = end;
            while (true)
            {
                while (Data[i] < pivot)
                {
                    i++;
                    PartitionSyncPoint(begin, end, i, j);
                }

                while (Data[j] > pivot)
                {
                    j--;
                    PartitionSyncPoint(begin, end, i, j);
                }

                if (i >= j)
                {
                    return j;
                }

                (Data[i], Data[j]) = (Data[j], Data[i]);
                PartitionSyncPoint(begin, end, i, j);
            }
        }
    }

    private void InsertionSort(int begin, int end)
    {
        for (int i = begin + 1; i < end; i++)
        {
            for (int j = i; j > begin; j--)
            {
                InsertionSyncPoint(begin, i, j);
                if (Data[j - 1] < Data[j])
                    break;
                
                (Data[j], Data[j - 1]) = (Data[j - 1], Data[j]);
            }
        }
    }

    private void SiftDown(int pos, int begin, int end)
    {
        int i = pos;
        while (true)
        {
            int max = i;
            int left = i * 2 - begin, right = left + 1;
            SiftSyncPoint(begin, end, pos, i, left, right);

            if (left < end && Data[left] > Data[max]) max = left;
            if (right < end && Data[right] > Data[max]) max = right;

            if (i == max)
                return;

            (Data[max], Data[i]) = (Data[i], Data[max]);
            i = max;
        }
    }

    private void HeapSort(int begin, int end)
    {
        for (int i = (begin + end) / 2 - 1; i >= begin; i--)
        {
            SiftDown(i, begin, end);
        }

        for (int i = end - 1; i >= begin; i--)
        {
            (Data[begin], Data[i]) = (Data[i], Data[begin]);
            ExtractSyncPoint(begin, end, begin, i);
            SiftDown(begin, begin, i);
        }
    }

    #region Utility functions

    private void FillHeapColours(int begin, int end)
    {
        for (int i = 0; i < Data.Length; i++)
        {
            if (i < begin || i >= end)
                Palette[i] = 0xFF_FFFFFF;
            else
            {
                int heapIdx = i - begin;
                Palette[i] = ((32 - int.LeadingZeroCount(heapIdx)) % 3) switch
                {
                    0 => 0xFF_FFFF80,
                    1 => 0xFF_80FFFF,
                    2 => 0xFF_FF80FF,
                    _ => 0xFF_FFFFFF
                };
            }
        }
    }

    #endregion

    #region Sync points

    private void PartitionSyncPoint(int begin, int end, int i, int j)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[begin..(end + 1)].Fill(0xFF_FF8080);
        Palette[begin..i].Fill(0xFF_FFFF80);
        Palette[i] = 0xFF_CC0000;
        Palette[j] = 0xFF_CC0000;
        Palette[(j + 1)..(end + 1)].Fill(0xFF_80FFFF);
        SyncPoint();
    }

    private void PivotSyncPoint(int begin, int end, int i)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[begin..(end + 1)].Fill(0xFF_FF8080);
        Palette[i] = 0xFF_CC0000;
        SyncPoint();
    }

    private void InsertionSyncPoint(int begin, int i, int j)
    {
        Palette.Fill(0xFF_FFFFFF);
        if (i >= 1)
            Palette[begin..i].Fill(0xFF_80FF80);
        
        Palette[i] = 0xFF_00CC00;
        Palette[j] = 0xFF_CC0000;
        if (j >= 1)
            Palette[j - 1] = 0xFF_CC0000;
        SyncPoint();
    }

    private void SiftSyncPoint(int begin, int end, int startPos, int pos, int left, int right)
    {
        FillHeapColours(begin, end);
        Palette[pos] = 0xFF_CC0000;
        Palette[startPos] = 0xFF_0066CC;
        if (left >= 0 && left < end)
            Palette[left] = 0xFF_00CC00;
        if (right >= 0 && right < end)
            Palette[right] = 0xFF_00CC00;

        SyncPoint();
    }

    private void ExtractSyncPoint(int begin, int end, int a, int b)
    {
        FillHeapColours(begin, end);
        Palette[a] = 0xFF_CC0000;
        Palette[b] = 0xFF_CC0000;
        
        SyncPoint();
    }

    #endregion
}