using System.Numerics;
using Silk.NET.OpenGL;
using SortingVisualizer.Rendering.OpenGL;
using SortingVisualizer.Sorting;
using VertexArray = SortingVisualizer.Rendering.OpenGL.VertexArray;

namespace SortingVisualizer.Rendering;

public class SceneManager
{
    private GL _gl;
    private BufferObject _dataBuffer;
    private BufferObject _paletteBuffer;
    private VertexArray _vertexArray;
    private ShaderProgram _shader;

    private SortingAlgorithm _algorithm;

    public SceneManager(GL gl, int length, Func<int, SortingAlgorithm> factory)
    {
        _gl = gl;
        _algorithm = factory(length);

        _dataBuffer = new BufferObject(_gl, VertexBufferObjectUsage.DynamicDraw);
        _dataBuffer.LoadData(_algorithm.Data);
        
        _paletteBuffer = new BufferObject(_gl, VertexBufferObjectUsage.DynamicDraw);
        _paletteBuffer.LoadData(_algorithm.Palette);

        _vertexArray = new VertexArray(_gl);
        _vertexArray.AttachVertexBuffer(0, _dataBuffer, sizeof(uint));
        _vertexArray.AttachVertexBuffer(1, _paletteBuffer, sizeof(uint));
        
        _vertexArray.SetupAttributeI(0,
            bindIndex: 0,
            count: 1,
            type: VertexAttribIType.UnsignedInt);
        _vertexArray.SetupAttributeI(0,
            bindIndex: 1,
            count: 1,
            type: VertexAttribIType.UnsignedInt);
        
        _shader = new ShaderProgram(_gl, new ShaderList
        {
            { ShaderType.VertexShader, new Uri("netres://SortingVisualizer/Assets/Shaders/bars.vert") },
            { ShaderType.GeometryShader, new Uri("netres://SortingVisualizer/Assets/Shaders/bars.geom") },
            { ShaderType.FragmentShader, new Uri("netres://SortingVisualizer/Assets/Shaders/bars.frag") }
        });
        _shader.Uniform(0, new Vector2(_algorithm.Data.Length, _algorithm.Data[^1]));
    }

    public void Update()
    {
        _dataBuffer.LoadData(_algorithm.Data);
        _paletteBuffer.LoadData(_algorithm.Palette);
    }

    public void Render()
    {
        _shader.MakeCurrent();
        _vertexArray.MakeCurrent();
        _gl.DrawArrays(PrimitiveType.Points, 0, 10);
    }
}