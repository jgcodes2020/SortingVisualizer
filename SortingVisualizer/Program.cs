using System.Drawing;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SortingVisualizer.Rendering;
using SortingVisualizer.Rendering.OpenGL;
using SortingVisualizer.Sorting.Basic;
using VertexArray = SortingVisualizer.Rendering.OpenGL.VertexArray;


namespace SortingVisualizer;

public static class Program
{
    private static readonly uint[] VertexData =
    {
        1, 
        2, 
        3, 
        4, 
        5, 
        6, 
        7, 
        8, 
        9, 
        10, 
    };

    private static readonly uint[] Palette =
    {
        0xFF_FF0000,
        0xFF_FF8800,
        0xFF_FFFF00,
        0xFF_00FF00,
        0xFF_00FFFF,
        0xFF_0000FF,
        0xFF_4400FF,
        0xFF_8800FF,
        0xFF_FF00FF,
        0xFF_FF0000,
    };
    
    
    private static IWindow _window = null!;
    private static IInputContext _input = null!;
    private static GL _gl = null!;

    private static SceneManager _scene;
    
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
        
        _scene = new SceneManager(_gl, 20, len => new InsertionSort((uint) len));
    }
    
    private static void OnUpdate(double dt)
    {
    }
    
    private static void OnRender(double dt)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        _scene.Render();
    }
}