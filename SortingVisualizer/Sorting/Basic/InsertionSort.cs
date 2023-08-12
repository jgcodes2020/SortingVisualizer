namespace SortingVisualizer.Sorting.Basic;

public class InsertionSort : SortingAlgorithm
{
    public InsertionSort(uint length) : base(length)
    {
    }

    protected override void DoSorting()
    {
        int i, j;
        for (i = 0; i < RawData.Length; i++)
        {
            for (j = i; j > 0; j--)
            {
                if (RawData[j - 1] <= RawData[j])
                    break;

                (RawData[j], RawData[j - 1]) = (RawData[j - 1], RawData[j]);
                TriggerSync();

                
            }
        }
        return;
        
        void TriggerSync()
        {
            Array.Fill(RawPalette, 0xFF_FFFFFF);
            RawPalette[i] = 0xFF_00CC00;
            RawPalette[j] = 0xFF_CC0000;
            if (j >= 1)
                RawPalette[j - 1] = 0xFF_CC0000;
            SyncPoint();
        }
    }
}