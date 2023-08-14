namespace SortingVisualizer.Sorting.Common;

public class QuickSort : SortingAlgorithm
{
    public QuickSort(BufferSet buffers) : base(buffers)
    {
    }

    protected override void DoSorting()
    {
        DoSorting(0, Data.Length - 1);
    }

    private void DoSorting(int begin, int end)
    {
        if (begin >= end)
            return;
        
        // Median-of-3 pivot selection
        if (end - begin >= 4)
        {
            int mid = (begin + end) / 2;

            if (Data[mid] > Data[begin] != Data[mid] > Data[end])
                (Data[begin], Data[mid]) = (Data[mid], Data[begin]);
            else if (Data[end] > Data[mid] != Data[end] > Data[begin])
                (Data[begin], Data[end]) = (Data[end], Data[begin]);
        }
        
        // Partitioning
        int curtain;
        {
            uint pivot = Data[begin];
            int i = begin, j = end;
            while (true)
            {
                while (Data[i] < pivot)
                {
                    i++;
                    SyncPoint(i, j);
                }

                while (Data[j] > pivot)
                {
                    j--;
                    SyncPoint(i, j);
                }

                if (i >= j)
                {
                    curtain = j;
                    break;
                }
            
                (Data[i], Data[j]) = (Data[j], Data[i]);
                SyncPoint(i, j);
            }
        }
        
        // j = partition point, recur into each half
        DoSorting(begin, curtain);
        DoSorting(curtain + 1, end);
    }

    private void SyncPoint(int i, int j)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[i] = 0xFF_CC0000;
        Palette[j] = 0xFF_CC0000;
        SyncPoint();
    }

    private void SyncPointBlank()
    {
        Palette.Fill(0xFF_FFFFFF);
        SyncPoint();
    }
}