using Silk.NET.OpenGL;

namespace SortingVisualizer.Rendering.OpenGL;

public sealed class BufferObject : IDisposable
{
    private GL _gl;
    internal readonly uint _handle;
    private VertexBufferObjectUsage _usage;

    public BufferObject(GL gl, VertexBufferObjectUsage usage)
    {
        _gl = gl;
        _handle = _gl.CreateBuffer();
        _usage = usage;
    }

    public void LoadData<T>(ReadOnlySpan<T> data, VertexBufferObjectUsage? usage = null) where T : unmanaged
    {
        _gl.NamedBufferData(_handle, data, usage ?? _usage);
        Span<int> x = new Span<int>(null);
        var y = (ReadOnlySpan<int>) x;
    }

    public void LoadData<T>(Span<T> data, VertexBufferObjectUsage? usage = null) where T : unmanaged
    {
        LoadData((ReadOnlySpan<T>) data, usage);
    }

    public void Dispose()
    {
        _gl.DeleteBuffer(_handle);
    }
}