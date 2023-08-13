using System.Diagnostics;
using SortingVisualizer.Misc;

namespace SortingVisualizer.Sorting;

/// <summary>
/// Base class representing a sorting algorithm.
/// </summary>
public abstract class SortingAlgorithm
{
    [Flags]
    private enum PlayState : uint
    {
        CategoryMask = 0xFFFF_0000,
        Stop = 0x0000_0000,
        Pause = 0x0000_0001,
        Run = 0x0001_0000,
        Advance = 0x0001_0001,
    }

    protected readonly uint[] _data;
    protected readonly uint[] _palette;
    public TimeSpan SyncDelay { get; set; }

    public ReadOnlySpan<uint> Data => _data;
    public ReadOnlySpan<uint> Palette => _palette;

    private PlayState _state;
    private bool _requestPauseState;
    private bool _stopRequested;
    private int _advanceRequested;
    private AutoResetEvent _advanceEvent;
    private ManualResetEvent _genericStopEvent;

    protected SortingAlgorithm(int length)
    {
        _data = new uint[length];
        _palette = new uint[length];
        SyncDelay = TimeSpan.FromMilliseconds(50);
        ArrayHelpers.FillRange(_data, 1U, 1U);
        Array.Fill(_palette, 0xFF_FFFFFFU);

        _state = PlayState.Stop;
        _advanceEvent = new AutoResetEvent(false);
        _genericStopEvent = new ManualResetEvent(false);
    }

    protected abstract void DoSorting();

    protected virtual void DoReset()
    {
        Array.Fill(_palette, 0xFF_FFFFFF);
    }

    public void StartSync()
    {
        _genericStopEvent.Reset();
        _state = PlayState.Run;
        try
        {
            DoSorting();
        }
        catch (OperationCanceledException)
        {
            // ignored
        }
        finally
        {
            _state = PlayState.Stop;
            _genericStopEvent.Set();
            DoReset();
        }
    }

    protected void SyncPoint()
    {
        if (_state == PlayState.Run)
        {
            if (!_advanceEvent.WaitOne(SyncDelay))
                return;

            if (_stopRequested)
            {
                throw new OperationCanceledException();
            }

            if (!_requestPauseState)
                return;
        }

        _state = PlayState.Pause;
        _genericStopEvent.Set();
        _advanceEvent.WaitOne();
        _genericStopEvent.Reset();
        if (_stopRequested)
        {
            throw new OperationCanceledException();
        }

        if (!_requestPauseState)
        {
            _state = PlayState.Run;
            return;
        }

        if (Interlocked.Exchange(ref _advanceRequested, 0) != 0)
        {
            _state = PlayState.Advance;
            return;
        }

        Debug.Fail("Pause notified, nothing changed!!");
    }

    public void Stop()
    {
        if (_state == PlayState.Stop)
            return;

        _stopRequested = true;
        _advanceEvent.Set();
        _genericStopEvent.WaitOne();
    }

    public void Shuffle()
    {
        Stop();
        ArrayHelpers.Shuffle(_data);
    }

    public void Reset()
    {
        Stop();
        Array.Sort(_data);
    }

    public void Pause()
    {
        if (_state == PlayState.Stop || _requestPauseState)
            return;

        _requestPauseState = true;
        _advanceEvent.Set();
        _genericStopEvent.WaitOne();
    }

    public void Resume()
    {
        if (_state == PlayState.Stop || !_requestPauseState)
            return;

        _requestPauseState = false;
        _advanceEvent.Set();
    }

    // STATIC HELPER METHODS
    // =====================

    public static SortingAlgorithm SelectInteractive(Action<SortingAlgorithm>? postInit = null)
    {
        var assembly = typeof(SortingAlgorithm).Assembly;
        var algList = assembly
            .GetTypes()
            .Where(t => t.Namespace!.StartsWith("SortingVisualizer.Sorting.") && t.IsSubclassOf(typeof(SortingAlgorithm)))
            .ToArray();
        Console.WriteLine("Available algorithms: ");
        foreach (var (alg, index) in algList.Select((x, i) => (x, i)))
        {
            Console.WriteLine($"{(index + 1),2}. {alg.FullName!.Replace("SortingVisualizer.Sorting.", "")}");
        }

        Type selectedAlg;
        while (true)
        {
            Console.Write($"Input the number of the algorithm [1-{algList.Length}]: ");
            if (!int.TryParse(Console.ReadLine()!, out var input))
            {
                Console.WriteLine("Not a valid number!");
                continue;
            }

            int trueIndex = input - 1;
            if (trueIndex >= algList.Length || trueIndex < 0)
            {
                Console.WriteLine("Index not in range!");
                continue;
            }

            selectedAlg = algList[trueIndex];
            break;
        }

        int length;
        while (true)
        {
            Console.Write("Input a POSITIVE length: ");
            if (!int.TryParse(Console.ReadLine()!, out length))
            {
                Console.WriteLine("Not a valid number!");
                continue;
            }

            if (length < 0)
            {
                Console.WriteLine("Not a valid length!");
                continue;
            }
            break;
        }

        TimeSpan syncDelay;
        while (true)
        {
            Console.Write("Input a sync time duration (like 5s 300ms 2.1us): ");
            try
            {
                syncDelay = ParseHelpers.ParseCombinedUnits(Console.ReadLine()!);
                break;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Invalid format!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid format...");
            }
        }

        var res = (SortingAlgorithm) Activator.CreateInstance(selectedAlg, length)!;
        res.SyncDelay = syncDelay;
        postInit?.Invoke(res);
        return res;
    }
}