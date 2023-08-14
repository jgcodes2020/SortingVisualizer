namespace SortingVisualizer.Sorting.Slow;

public class BubbleSort : SortingAlgorithm
{
    public BubbleSort(BufferSet buffers) : base(buffers)
    {
    }

    protected override void DoSorting()
    {
        bool sorted;
        for (int i = Data.Length - 1; i > 0; i--)
        {
            sorted = true;
            for (int j = 0; j < i; j++)
            {
                SyncPoint(i, j);
                if (Data[j] > Data[j + 1])
                {
                    (Data[j], Data[j + 1]) = (Data[j + 1], Data[j]);
                    sorted = false;
                }
            }
            if (sorted)
                break;
        }
    }

    private void SyncPoint(int i, int j)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[i] = 0xFF_00CC00;
        Palette[j] = 0xFF_CC0000;
        Palette[j + 1] = 0xFF_CC0000;
        SyncPoint();
    }

}