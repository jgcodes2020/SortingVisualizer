using System.Drawing;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SortingVisualizer.Rendering;
using SortingVisualizer.Rendering.OpenGL;
using VertexArray = SortingVisualizer.Rendering.OpenGL.VertexArray;

namespace SortingVisualizer;

public static class Program
{
    private static readonly uint[] VertexData =
    {
        1, 0xFF_FF0000,
        2, 0xFF_FF8800,
        3, 0xFF_FFFF00,
        4, 0xFF_00FF00,
        5, 0xFF_00FFFF,
        6, 0xFF_0000FF,
        7, 0xFF_4400FF,
        8, 0xFF_8800FF,
        9, 0xFF_FF00FF,
        10, 0xFF_FF0000,
    };
    
    private static IWindow _window = null!;
    private static IInputContext _input = null!;
    private static GL _gl = null!;

    private static VertexArray _vao;
    private static BufferObject _vbo;
    private static ShaderProgram _shader;
    
    public static void Main(string[] args)
    {
        Console.WriteLine("Available resources:");
        foreach (var resourceName in typeof(Program).Assembly.GetManifestResourceNames())
        {
            Console.WriteLine($"- {resourceName}");
        }
        
        _window = Window.Create(WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "Sorting Visualizer"
        });
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        
        _window.Run();
    }


    private static void OnLoad()
    {
        _window.Center();
        _gl = _window.CreateOpenGL();
        _input = _window.CreateInput();
        
        _window.MakeCurrent();
        _gl.CheckVersion(4, 5);
        
        InitOpenGL();
    }

    private static unsafe void InitOpenGL()
    {
        _gl.ClearColor(Color.Black);
        
        _shader = new ShaderProgram(_gl, new ShaderList
        {
            { ShaderType.VertexShader, new Uri("netres://SortingVisualizer/Assets/Shaders/bars.vert") },
            { ShaderType.GeometryShader, new Uri("netres://SortingVisualizer/Assets/Shaders/bars.geom") },
            { ShaderType.FragmentShader, new Uri("netres://SortingVisualizer/Assets/Shaders/bars.frag") }
        });
        _shader.Uniform(0, new Vector2(10.0f, 10.0f));
        
        _vbo = new BufferObject(_gl, VertexBufferObjectUsage.StaticDraw);
        _vbo.LoadData<uint>(VertexData);

        _vao = new VertexArray(_gl);
        _vao.AttachVertexBuffer(0, 
            buffer: _vbo, 
            elementSize: 2 * sizeof(uint));
        _vao.SetupAttributeI(0, 
            bindIndex: 0, 
            count: 1, 
            type: VertexAttribIType.UnsignedInt, 
            offset: 0);
        _vao.SetupAttributeI(1, 
            bindIndex: 0, 
            count: 1, 
            type: VertexAttribIType.UnsignedInt, 
            offset: 1 * sizeof(uint));
    }
    
    private static void OnUpdate(double dt)
    {
    }
    
    private static void OnRender(double dt)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        
        _shader.MakeCurrent();
        _vao.MakeCurrent();
        _gl.Viewport(_window.Size);
        _gl.DrawArrays(PrimitiveType.Points, 0, 10);
    }
}