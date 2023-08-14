namespace SortingVisualizer.Sorting.Common;

public class InsertionSort : SortingAlgorithm
{
    public InsertionSort(BufferSet buffers) : base(buffers)
    {
    }

    protected override void DoSorting()
    {
        int i, j;
        for (i = 0; i < Data.Length; i++)
        {
            for (j = i; j > 0; j--)
            {
                if (Data[j - 1] <= Data[j])
                    break;

                (Data[j], Data[j - 1]) = (Data[j - 1], Data[j]);
                SyncPoint(i, j);
            }
        }
        return;
    }

    private void SyncPoint(int i, int j)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[i] = 0xFF_00CC00;
        Palette[j] = 0xFF_CC0000;
        if (j >= 1)
            Palette[j - 1] = 0xFF_CC0000;
        SyncPoint();
    }
}