namespace SortingVisualizer.Sorting.Common;

public class MergeSort : SortingAlgorithm
{
    private uint[] _dTemp;

    public MergeSort(BufferSet buffers) : base(buffers)
    {
        _dTemp = new uint[buffers.Length];
    }

    public override BufferSet Buffers
    {
        get => base.Buffers;
        set
        {
            base.Buffers = value;
            _dTemp = new uint[value.Length];
        }
    }

    protected override void DoSorting()
    {
        DoSorting(0, Data.Length);
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
                if (Data[il] <= Data[ir])
                {
                    _dTemp[it] = Data[il];
                    il++;
                }
                else
                {
                    _dTemp[it] = Data[ir];
                    ir++;
                }

                it++;
            }

            while (il < mid)
            {
                SyncPoint(il);
                _dTemp[it] = Data[il];
                il++;
                it++;
            }

            while (ir < end)
            {
                SyncPoint(ir);
                _dTemp[it] = Data[ir];
                ir++;
                it++;
            }
            SyncPoint(il, ir);
        }
        // copy back
        for (int i = 0; i < len; i++)
        {
            Data[begin + i] = _dTemp[i];
            SyncPoint(begin +i);
        }
    }

    private void SyncPoint(int a, int? b = null)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[a] = 0xFF_CC0000;
        if (b.HasValue && b < Data.Length)
            Palette[b.Value] = 0xFF_CC0000;
        SyncPoint();
    }
}