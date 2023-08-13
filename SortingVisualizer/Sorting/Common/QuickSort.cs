namespace SortingVisualizer.Sorting.Common;

public class QuickSort : SortingAlgorithm
{
    public QuickSort(int length) : base(length)
    {
    }

    protected override void DoSorting()
    {
        DoSorting(0, _data.Length - 1);
    }

    private void DoSorting(int begin, int end)
    {
        if (begin >= end)
            return;
        
        // Median-of-3 pivot selection
        if (end - begin >= 4)
        {
            int mid = (begin + end) / 2;

            if (_data[mid] > _data[begin] != _data[mid] > _data[end])
                (_data[begin], _data[mid]) = (_data[mid], _data[begin]);
            else if (_data[end] > _data[mid] != _data[end] > _data[begin])
                (_data[begin], _data[end]) = (_data[end], _data[begin]);
        }
        
        // Partitioning
        int curtain;
        {
            uint pivot = _data[begin];
            int i = begin, j = end;
            while (true)
            {
                while (_data[i] < pivot)
                {
                    i++;
                    SyncPoint(i, j);
                }

                while (_data[j] > pivot)
                {
                    j--;
                    SyncPoint(i, j);
                }

                if (i >= j)
                {
                    curtain = j;
                    break;
                }
            
                (_data[i], _data[j]) = (_data[j], _data[i]);
                SyncPoint(i, j);
            }
        }
        
        // j = partition point, recur into each half
        DoSorting(begin, curtain);
        DoSorting(curtain + 1, end);
    }

    private void SyncPoint(int i, int j)
    {
        Array.Fill(_palette, 0xFF_FFFFFF);
        _palette[i] = 0xFF_CC0000;
        _palette[j] = 0xFF_CC0000;
        SyncPoint();
    }

    private void SyncPointBlank()
    {
        Array.Fill(_palette, 0xFF_FFFFFF);
        SyncPoint();
    }
}