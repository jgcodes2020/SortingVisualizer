namespace SortingVisualizer.Sorting.Slow;

public class SelectionSort : SortingAlgorithm
{
    public SelectionSort(BufferSet buffers) : base(buffers)
    {
    }

    protected override void DoSorting()
    {
        for (int i = 0; i < Data.Length - 1; i++)
        {
            int min = i;
            for (int j = i; j < Data.Length; j++)
            {
                LoopSyncPoint(i, j, min);
                if (Data[j] < Data[min])
                    min = j;
            }

            SwapSyncPoint(i, min);
            (Data[i], Data[min]) = (Data[min], Data[i]);
        }
    }

    private void LoopSyncPoint(int i, int j, int min)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[..i].Fill(0xFF_80FF80);
        Palette[j] = 0xFF_CC0000;
        Palette[min] = 0xFF_00CC00;
        SyncPoint();
    }
    
    private void SwapSyncPoint(int i, int min)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[..i].Fill(0xFF_80FF80);
        Palette[i] = 0xFF_CC0000;
        Palette[min] = 0xFF_CC0000;
        SyncPoint();
    }
}