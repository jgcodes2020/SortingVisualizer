using System.Drawing;
using System.Numerics;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
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

    private static ImGuiController _gui;
    
    public static void Main(string[] args)
    {
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

        _gui = new ImGuiController(_gl, _window, _input);
        
        _window.MakeCurrent();
        _gl.CheckVersion(4, 5);
        
        InitOpenGL();
    }

    private static unsafe void InitOpenGL()
    {
        _gl.ClearColor(Color.Black);
    }
    
    private static void OnUpdate(double dt)
    {
        _gui.Update((float) dt);
        using (ImGuiExt.FillViewport())
        {
            ImGui.Text("Hello, world!");
        }
    }
    
    private static void OnRender(double dt)
    {
        _gl.Viewport(new Vector2D<int>(0, 0), _window.FramebufferSize);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        _gui.Render();
    }
}