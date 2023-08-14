using System.Drawing;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SortingVisualizer.Rendering;
using SortingVisualizer.Rendering.OpenGL;
using SortingVisualizer.Sorting;
using SortingVisualizer.Sorting.Common;
using VertexArray = SortingVisualizer.Rendering.OpenGL.VertexArray;


namespace SortingVisualizer;

public static class Program
{
    private static IWindow _window = null!;
    private static IInputContext _input = null!;
    private static GL _gl = null!;

    private static SceneManager _scene;
    private static SortingAlgorithm _algorithm;
    
    public static void Main(string[] args)
    {
        _algorithm = new HeapSort(30) { SyncDelay = TimeSpan.FromMilliseconds(30) };
        
        _window = Window.Create(WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "Sorting Visualizer",
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
        
        _scene = new SceneManager(_gl, _input, _algorithm);
    }
    
    private static void OnUpdate(double dt)
    {
        _scene.Update(_input);
    }
    
    private static void OnRender(double dt)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        _scene.Render();
    }
}