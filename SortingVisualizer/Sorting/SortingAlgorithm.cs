using System.Diagnostics;
using System.Runtime.InteropServices;
using SortingVisualizer.Misc;

namespace SortingVisualizer.Sorting;

/// <summary>
/// Base class representing a sorting algorithm.
/// </summary>
public abstract class SortingAlgorithm
{
    public delegate void ShuffleCallback(Span<uint> data);

    public class BufferSet
    {
        public BufferSet(Memory<uint> data, Memory<uint> palette)
        {
            if (data.Length != palette.Length)
                throw new ArgumentException("");
            Data = data;
            Palette = palette;

            MaxValue = MemoryMarshal.ToEnumerable<uint>(data).Max();
        }

        public Memory<uint> Data { get; }
        public Memory<uint> Palette { get; }

        public int Length => Data.Length;
        public uint MaxValue { get; }

        public static BufferSet Sequence(int n)
        {
            var data = new uint[n];
            var palette = new uint[n];
            
            ArrayHelpers.FillRange(data, 1u, 1u);
            Array.Fill(palette, 0xFF_FFFFFF);

            return new BufferSet(data, palette);
        }
    }

    [Flags]
    private enum PlayState : uint
    {
        CategoryMask = 0xFFFF_0000,
        Stop = 0x0000_0000,
        Pause = 0x0000_0001,
        Run = 0x0001_0000,
        Advance = 0x0001_0001,
    }

    private BufferSet _buffers;
    public TimeSpan SyncDelay { get; set; }

    public Span<uint> Data => Buffers.Data.Span;
    public Span<uint> Palette => Buffers.Palette.Span;

    public bool IsRunning => _state != PlayState.Stop;

    public virtual BufferSet Buffers
    {
        get => _buffers;
        set => _buffers = value;
    }

    private PlayState _state;
    private bool _requestPauseState;
    private bool _stopRequested;
    private int _advanceRequested;
    private AutoResetEvent _advanceEvent;
    private ManualResetEvent _genericStopEvent;

    protected SortingAlgorithm(BufferSet buffers)
    {
        Buffers = buffers;
        SyncDelay = TimeSpan.FromMilliseconds(50);
        Data.FillRange(1U, 1U);
        Palette.Fill(0xFF_FFFFFFU);

        _state = PlayState.Stop;
        _advanceEvent = new AutoResetEvent(false);
        _genericStopEvent = new ManualResetEvent(false);
    }

    protected abstract void DoSorting();

    protected virtual void DoReset()
    {
        Palette.Fill(0xFF_FFFFFF);
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

    public void Reset(ShuffleCallback permuter)
    {
        Stop();
        permuter(Data);
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
}