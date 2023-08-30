namespace SortingVisualizer.Sorting.Slow;

/// <summary>
/// Stooge sort. Works by recursively sorting the intervals [0, 2n/3), [n/3, n), [0, 2n/3).
/// Has an insanely high time complexity.
/// </summary>
public class StoogeSort : SortingAlgorithm
{
    public StoogeSort(BufferSet buffers) : base(buffers)
    {
    }

    protected override void DoSorting()
    {
        DoSorting(0, Data.Length);
    }

    private void CompareSwap(int a, int b)
    {
        if (Data[a] > Data[b])
            (Data[a], Data[b]) = (Data[b], Data[a]);
        
        Palette.Fill(0xFF_FFFFFF);
        Palette[a] = 0xFF_CC0000;
        Palette[b] = 0xFF_CC0000;
        SyncPoint();
    }

    private void DoSorting(int begin, int end)
    {
        int len = end - begin;

        switch (len)
        {
            case <= 1:
                break;
            case 2:
                CompareSwap(begin, begin + 1);
                break;
            case 3:
                CompareSwap(begin + 0, begin + 1);
                CompareSwap(begin + 1, begin + 2);
                CompareSwap(begin + 0, begin + 1);
                break;
            default:
            {
                int leftMid = begin + len / 3;
                int rightMid = begin + (len * 2 + 2) / 3;
        
                DoSorting(begin, rightMid);
                DoSorting(leftMid, end);
                DoSorting(begin, rightMid);
                break;
            }
        }
    }
}