namespace SortingVisualizer.Sorting.Common;

public class IterativeMergeSort : SortingAlgorithm
{
    private uint[] _dTemp;

    public IterativeMergeSort(BufferSet buffers) : base(buffers)
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
        for (int half = 1; half < Data.Length; half *= 2)
        {
            Console.WriteLine($"step = {half}, len = {Data.Length}");
            int step = half * 2;
            int j;
            for (j = 0; j < Data.Length; j += step)
            {
                int end = Math.Min(Data.Length, j + step);
                if (end - j > half)
                    Merge(j, j + half, end);
            }
        }
        
    }

    private void Merge(int begin, int mid, int end)
    {
        int len = end - begin;
        
        int it = 0;
        int il = begin, ir = mid;
        while (il < mid && ir < end)
        {
            MergeSyncPoint(begin, mid, end, il, ir);
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
            MergeSyncPoint(begin, mid, end, il);
            _dTemp[it] = Data[il];
            il++;
            it++;
        }

        while (ir < end)
        {
            MergeSyncPoint(begin, mid, end, ir);
            _dTemp[it] = Data[ir];
            ir++;
            it++;
        }
        MergeSyncPoint(begin, mid, end, il, ir);
        
        for (int i = 0; i < len; i++)
        {
            Data[begin + i] = _dTemp[i];
            CopySyncPoint(begin, end, begin + i);
        }
    }
    
    private void MergeSyncPoint(int begin, int mid, int end, int a, int? b = null)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[begin..mid].Fill(0xFF_FFFF80);
        Palette[mid..end].Fill(0xFF_80FFFF);
        
        Palette[a] = 0xFF_CC0000;
        if (b < end)
            Palette[b.Value] = 0xFF_CC0000;
        SyncPoint();
    }

    private void CopySyncPoint(int begin, int end, int i)
    {
        Palette.Fill(0xFF_FFFFFF);
        Palette[begin..i].Fill(0xFF_80FF80);
        Palette[i] = 0xFF_CC0000;
        
        SyncPoint();
    }
}