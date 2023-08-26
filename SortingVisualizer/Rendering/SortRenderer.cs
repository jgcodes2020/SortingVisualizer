using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using SortingVisualizer.Misc;
using SortingVisualizer.Rendering.OpenGL;
using SortingVisualizer.Sorting;
using VertexArray = SortingVisualizer.Rendering.OpenGL.VertexArray;

namespace SortingVisualizer.Rendering;

public class SortRenderer
{
    private GL _gl;
    private BufferObject _dataBuffer;
    private BufferObject _paletteBuffer;
    private VertexArray _vertexArray;
    private ShaderProgram _shader;

    public SortRenderer(GL gl, SortingAlgorithm? algorithm)
    {
        Algorithm = algorithm;
        InitOpenGL(gl);
    }

    public SortingAlgorithm? Algorithm { get; set; }

    [MemberNotNull(nameof(_gl), nameof(_dataBuffer), nameof(_paletteBuffer))]
    [MemberNotNull(nameof(_vertexArray), nameof(_shader))]
    private void InitOpenGL(GL gl)
    {
        _gl = gl;
        
        _dataBuffer = new BufferObject(_gl, VertexBufferObjectUsage.DynamicDraw);
        _paletteBuffer = new BufferObject(_gl, VertexBufferObjectUsage.DynamicDraw);

        _vertexArray = new VertexArray(_gl);
        _vertexArray.AttachVertexBuffer(0, 
            buffer: _dataBuffer, 
            elementSize: sizeof(uint));
        _vertexArray.AttachVertexBuffer(1, 
            buffer: _paletteBuffer, 
            elementSize: sizeof(uint));
        _vertexArray.SetupAttributeI(0,
            bindIndex: 0,
            count: 1,
            type: VertexAttribIType.UnsignedInt,
            offset: 0);
        _vertexArray.SetupAttributeI(1,
            bindIndex: 1,
            count: 1,
            type: VertexAttribIType.UnsignedInt,
            offset: 0);
        
        _shader = new ShaderProgram(_gl, new ShaderList
        {
            { ShaderType.VertexShader, new Uri("netres://SortingVisualizer/Assets/Shaders/bars.vert") },
            { ShaderType.GeometryShader, new Uri("netres://SortingVisualizer/Assets/Shaders/bars.geom") },
            { ShaderType.FragmentShader, new Uri("netres://SortingVisualizer/Assets/Shaders/bars.frag") }
        });

        if (Algorithm != null)
        {
            _dataBuffer.LoadData(Algorithm.Data);
            _paletteBuffer.LoadData(Algorithm.Palette);
            _shader.Uniform(0, new Vector2(Algorithm.Buffers.Length, Algorithm.Buffers.MaxValue));
        }
    }

    public void Render()
    {
        if (Algorithm == null)
            return;
        _dataBuffer.LoadData(Algorithm.Data);
        _paletteBuffer.LoadData(Algorithm.Palette);
        _shader.Uniform(0, new Vector2(Algorithm.Buffers.Length, Algorithm.Buffers.MaxValue));
        
        _shader.MakeCurrent();
        _vertexArray.MakeCurrent();
        _gl.DrawArrays(PrimitiveType.Points, 0, (uint) Algorithm.Buffers.Length);
    }
}