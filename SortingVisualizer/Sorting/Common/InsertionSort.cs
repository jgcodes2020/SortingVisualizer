namespace SortingVisualizer.Sorting.Common;

public class InsertionSort : SortingAlgorithm
{
    public InsertionSort(int length) : base(length)
    {
    }

    protected override void DoSorting()
    {
        int i, j;
        for (i = 0; i < _data.Length; i++)
        {
            for (j = i; j > 0; j--)
            {
                if (_data[j - 1] <= _data[j])
                    break;

                (_data[j], _data[j - 1]) = (_data[j - 1], _data[j]);
                SyncPoint(i, j);
            }
        }
        return;
    }

    private void SyncPoint(int i, int j)
    {
        Array.Fill(_palette, 0xFF_FFFFFF);
        _palette[i] = 0xFF_00CC00;
        _palette[j] = 0xFF_CC0000;
        if (j >= 1)
            _palette[j - 1] = 0xFF_CC0000;
        SyncPoint();
    }
}