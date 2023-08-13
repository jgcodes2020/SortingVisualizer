namespace SortingVisualizer.Sorting.Common;

public class MergeSort : SortingAlgorithm
{
    private uint[] _dTemp;
    
    public MergeSort(int length) : base(length)
    {
        _dTemp = new uint[length];
    }

    protected override void DoSorting()
    {
        DoSorting(0, _data.Length);
    }

    private void DoSorting(int begin, int end)
    {
        if (begin + 1 >= end)
            return;

        int mid = (begin + end) / 2;
        int len = end - begin;
        
        DoSorting(begin, mid);
        DoSorting(mid, end);
        
        // merge the two arrays
        {
            int it = 0;
            int il = begin, ir = mid;
            while (il < mid && ir < end)
            {
                SyncPoint(il, ir);
                if (_data[il] <= _data[ir])
                {
                    _dTemp[it] = _data[il];
                    il++;
                }
                else
                {
                    _dTemp[it] = _data[ir];
                    ir++;
                }

                it++;
            }

            while (il < mid)
            {
                SyncPoint(il);
                _dTemp[it] = _data[il];
                il++;
                it++;
            }

            while (ir < end)
            {
                SyncPoint(ir);
                _dTemp[it] = _data[ir];
                ir++;
                it++;
            }
            SyncPoint(il, ir);
        }
        // copy back
        for (int i = 0; i < len; i++)
        {
            _data[begin + i] = _dTemp[i];
            SyncPoint(begin +i);
        }
    }

    private void SyncPoint(int a, int? b = null)
    {
        Array.Fill(_palette, 0xFF_FFFFFF);
        _palette[a] = 0xFF_CC0000;
        if (b.HasValue && b < Data.Length)
            _palette[b.Value] = 0xFF_CC0000;
        SyncPoint();
    }
}