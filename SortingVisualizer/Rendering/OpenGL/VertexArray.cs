using System.Collections;
using System.Collections.Specialized;
using System.Numerics;
using Silk.NET.OpenGL;
using SortingVisualizer.Misc;

namespace SortingVisualizer.Rendering.OpenGL;

public sealed class VertexArray : IDisposable
{
    private GL _gl;
    internal readonly uint _handle;
    private FixedBitSet _openSlots;

    public VertexArray(GL gl)
    {
        _gl = gl;
        _handle = _gl.CreateVertexArray();
        _openSlots = new FixedBitSet(256);
    }

    public void MakeCurrent()
    {
        _gl.BindVertexArray(_handle);
    }

    public void AttachVertexBuffer(uint bindIndex, BufferObject buffer, uint elementSize, int baseOffset = 0)
    {
        _gl.VertexArrayVertexBuffer(_handle, bindIndex, buffer._handle, baseOffset, elementSize);
    }

    public void DetachVertexBuffer(uint bindIndex)
    {
        _gl.VertexArrayVertexBuffer(_handle, bindIndex, 0, 0, 0);
    }

    public void AttachElementBuffer(BufferObject buffer)
    {
        _gl.VertexArrayElementBuffer(_handle, buffer._handle);
    }

    public void DetachElementBuffer()
    {
        _gl.VertexArrayElementBuffer(_handle, 0);
    }

    public void AttributeEnable(uint location)
    {
        _gl.EnableVertexArrayAttrib(_handle, location);
    }

    public void AttributeBind(uint location, uint bindIndex)
    {
        _gl.VertexArrayAttribBinding(_handle, location, bindIndex);
    }

    public void AttributeFormat(uint location, int count, VertexAttribType type, bool normalized, uint relativeOffset)
    {
        _gl.VertexArrayAttribFormat(_handle, location, count, type, normalized, relativeOffset);
    }

    public void AttributeIFormat(uint location, int count, VertexAttribIType type, uint relativeOffset)
    {
        _gl.VertexArrayAttribIFormat(_handle, location, count, type, relativeOffset);
    }

    public void Dispose()
    {
        _gl.DeleteVertexArray(_handle);
    }
}