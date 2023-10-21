namespace SortingVisualizer.Sorting.Slow;

/// <summary>
/// Unoptimized bubble sort, as implemented by my CS teachers.
/// It does not take advantage of bubble sort bringing one element to the top each time.
/// In addition, it's very easy to add an early break if the array is already sorted, but
/// this is not done either.
/// </summary>
public class WorseBubbleSort : SortingAlgorithm
{
    public WorseBubbleSort(BufferSet buffers) : base(buffers)
    {
    }
    
    protected override void DoSorting()
    {
        for (int i = 0; i < Data.Length - 1; i++)
        {
            for (int j = 0; j < Data.Length - 1; j++)
            {
                SyncPoint(j);
                if (Data[j] > Data[j + 1])
                    (Data[j], Data[j + 1]) = (Data[j + 1], Data[j]);
            }
        }
    }

    private void SyncPoint(int j)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[j] = 0xFF_CC0000;
        Palette[j + 1] = 0xFF_CC0000;
        SyncPoint();
    }
}