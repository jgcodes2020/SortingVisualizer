namespace SortingVisualizer.Sorting.Hybrid;

public class CppStableSort : SortingAlgorithm
{
    private uint[] _dTemp;

    public CppStableSort(BufferSet buffers) : base(buffers)
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
        const int INSERT_SORT_LIMIT = 16;
        
        for (int i = 0; i < Data.Length; i += INSERT_SORT_LIMIT)
        {
            InsertionSort(i, Math.Min(i + INSERT_SORT_LIMIT, Data.Length));
        }
        
        for (int half = INSERT_SORT_LIMIT; half < Data.Length; half *= 2)
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
    
    private void InsertionSort(int begin, int end)
    {
        for (int i = begin + 1; i < end; i++)
        {
            for (int j = i; j > begin; j--)
            {
                InsertionSyncPoint(begin, i, j);
                if (Data[j - 1] < Data[j])
                    break;
                
                (Data[j], Data[j - 1]) = (Data[j - 1], Data[j]);
            }
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
    
    private void InsertionSyncPoint(int begin, int i, int j)
    {
        Palette.Fill(0xFF_FFFFFF);
        if (i >= 1)
            Palette[begin..i].Fill(0xFF_80FF80);
        
        Palette[i] = 0xFF_00CC00;
        Palette[j] = 0xFF_CC0000;
        if (j >= 1)
            Palette[j - 1] = 0xFF_CC0000;
        SyncPoint();
    }
    
}