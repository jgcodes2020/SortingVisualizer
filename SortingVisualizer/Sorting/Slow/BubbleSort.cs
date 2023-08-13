namespace SortingVisualizer.Sorting.Slow;

public class BubbleSort : SortingAlgorithm
{
    public BubbleSort(int length) : base(length)
    {
    }

    protected override void DoSorting()
    {
        bool sorted;
        for (int i = _data.Length - 1; i > 0; i--)
        {
            sorted = true;
            for (int j = 0; j < i; j++)
            {
                SyncPoint(i, j);
                if (_data[j] > _data[j + 1])
                {
                    (_data[j], _data[j + 1]) = (_data[j + 1], _data[j]);
                    sorted = false;
                }
            }
            if (sorted)
                break;
        }
    }

    private void SyncPoint(int i, int j)
    {
        Array.Fill(_palette, 0xFF_FFFFFF);
        _palette[i] = 0xFF_00CC00;
        _palette[j] = 0xFF_CC0000;
        _palette[j + 1] = 0xFF_CC0000;
        SyncPoint();
    }
}