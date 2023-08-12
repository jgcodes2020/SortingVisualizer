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
        DoSorting(0, RawData.Length);
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
                if (RawData[il] <= RawData[ir])
                {
                    _dTemp[it] = RawData[il];
                    il++;
                }
                else
                {
                    _dTemp[it] = RawData[ir];
                    ir++;
                }

                it++;
            }

            while (il < mid)
            {
                SyncPoint(il);
                _dTemp[it] = RawData[il];
                il++;
                it++;
            }

            while (ir < end)
            {
                SyncPoint(ir);
                _dTemp[it] = RawData[ir];
                ir++;
                it++;
            }
            SyncPoint(il, ir);
        }
        // copy back
        for (int i = 0; i < len; i++)
        {
            RawData[begin + i] = _dTemp[i];
            SyncPoint(begin +i);
        }
    }

    private void SyncPoint(int a, int? b = null)
    {
        Array.Fill(RawPalette, 0xFF_FFFFFF);
        RawPalette[a] = 0xFF_CC0000;
        if (b.HasValue && b < Data.Length)
            RawPalette[b.Value] = 0xFF_CC0000;
        SyncPoint();
    }
}