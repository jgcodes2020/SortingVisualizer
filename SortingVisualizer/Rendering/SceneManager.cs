using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using SortingVisualizer.Misc;
using SortingVisualizer.Rendering.OpenGL;
using SortingVisualizer.Sorting;
using VertexArray = SortingVisualizer.Rendering.OpenGL.VertexArray;

namespace SortingVisualizer.Rendering;

public class SceneManager
{
    [Flags]
    private enum KeybindMask
    {
        None = 0,
        Shuffle = (1 << 0),
        Start = (1 << 1),
    }
    
    private GL _gl;
    private BufferObject _dataBuffer;
    private BufferObject _paletteBuffer;
    private VertexArray _vertexArray;
    private ShaderProgram _shader;

    private KeybindMask _tracker;
    
    private SortingAlgorithm _algorithm;

    public SceneManager(GL gl, IInputContext input, SortingAlgorithm algorithm)
    {
        _algorithm = algorithm;
        InitOpenGL(gl);
        InitInput(input);
    }

    [MemberNotNull(nameof(_gl), nameof(_dataBuffer), nameof(_paletteBuffer))]
    [MemberNotNull(nameof(_vertexArray), nameof(_shader))]
    private void InitOpenGL(GL gl)
    {
        _gl = gl;

        _dataBuffer = new BufferObject(_gl, VertexBufferObjectUsage.DynamicDraw);
        _dataBuffer.LoadData(_algorithm.Data);
        
        _paletteBuffer = new BufferObject(_gl, VertexBufferObjectUsage.DynamicDraw);
        _paletteBuffer.LoadData(_algorithm.Palette);

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
        _shader.Uniform(0, new Vector2(_algorithm.Data.Length, _algorithm.Data[^1]));
    }

    private void InitInput(IInputContext input)
    {
        _tracker = KeybindMask.None;
        foreach (var kbd in input.Keyboards)
        {
            kbd.KeyDown += OnKeyDown;
        }
    }

    private void OnKeyDown(IKeyboard kbd, Key key, int scancode)
    {
        switch (key)
        {
            case Key.F1:
                InterlockedExt.Or(ref _tracker, KeybindMask.Shuffle);
                break;
            case Key.F2:
                InterlockedExt.Or(ref _tracker, KeybindMask.Start);
                break;
        }
    }

    public void Update(IInputContext input)
    {
        if ((InterlockedExt.And(ref _tracker, ~KeybindMask.Shuffle) & KeybindMask.Shuffle) != 0)
        {
            _algorithm.Shuffle();
        }

        if ((InterlockedExt.And(ref _tracker, ~KeybindMask.Start) & KeybindMask.Start) != 0)
        {
            Task.Run(() => _algorithm.StartSync());
        }
    }

    public void Render()
    {
        _dataBuffer.LoadData(_algorithm.Data);
        _paletteBuffer.LoadData(_algorithm.Palette);
        
        _shader.MakeCurrent();
        _vertexArray.MakeCurrent();
        _gl.DrawArrays(PrimitiveType.Points, 0, (uint) _algorithm.Data.Length);
    }
}