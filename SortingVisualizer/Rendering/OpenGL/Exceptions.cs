using Silk.NET.OpenGL;

namespace SortingVisualizer.Rendering.OpenGL;

/// <summary>
/// Serves as a base class for all OpenGL exceptions.
/// </summary>
public class GLException : Exception
{
    public GLException()
    {
    }

    public GLException(string? message) : base(message)
    {
    }

    public GLException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

/// <summary>
/// Thrown when shader compilation or linking fails.
/// </summary>
public class ShaderBuildException : GLException
{
    public ObjectType Type { get; }

    public enum ObjectType : int
    {
        Program = GLEnum.Program,
        VertexShader = GLEnum.VertexShader,
        FragmentShader = GLEnum.FragmentShader,
        GeometryShader = GLEnum.GeometryShader,
        TessControlShader = GLEnum.TessControlShader,
        TessEvaluationShader = GLEnum.TessEvaluationShader,
        ComputeShader = GLEnum.ComputeShader
    }

    public ShaderBuildException(ObjectType type, string log) : base($"{type}: {log}")
    {
        Type = type;
    }
}