using System.Drawing;
using System.Numerics;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using SortingVisualizer.Misc;
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

    private static ImGuiController _imGui;
    private static UIManager _ui;
    private static SortRenderer _sort;
    
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

        _imGui = new ImGuiController(_gl, _window, _input);

        _ui = new UIManager();
        {
            var alg = (SortingAlgorithm) Activator.CreateInstance(_ui.CurrentAlgorithm,
                SortingAlgorithm.BufferSet.Sequence(_ui.DataLength))!;
            
            _sort = new SortRenderer(_gl, alg);
        }
        
        _window.MakeCurrent();
        
        _gl.CheckVersion(4, 5);
        _gl.ClearColor(Color.Black);
    }
    
    private static void OnUpdate(double dt)
    {
        _imGui.Update((float) dt);
        _ui.SetDrawList(_window.Size);

        if (_ui.ParametersUpdated())
        {
            _sort.Algorithm?.Stop();
            _sort.Algorithm = (SortingAlgorithm) Activator.CreateInstance(_ui.CurrentAlgorithm,
                SortingAlgorithm.BufferSet.Sequence(_ui.DataLength))!;
        }

        if (_ui.ShufflePressed())
        {
            _sort.Algorithm?.Reset(span => span.Shuffle());
        }

        if (_ui.StartPressed())
        {
            if (!_sort.Algorithm?.IsRunning ?? false)
                Task.Run(() => _sort.Algorithm?.StartSync());
        }
    }
    
    private static void OnRender(double dt)
    {
        _gl.Viewport(_window.Size);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        _imGui.Render();

        _gl.Viewport(UIManager.DockWidth, 0, (uint) (_window.Size.X - UIManager.DockWidth), (uint) _window.Size.Y);
        _sort.Render();
        
        _gl.Viewport(_window.Size);
    }
}