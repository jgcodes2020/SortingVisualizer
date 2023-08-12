using System.Diagnostics;
using Silk.NET.OpenGL;

namespace SortingVisualizer.Rendering.OpenGL;

public static class GLExtensions
{
    [Conditional("DEBUG")]
    public static void AssertNoError(this GL gl)
    {
        var err = (ErrorCode) gl.GetError();
        Debug.Assert(err == ErrorCode.NoError, "Unexpected OpenGL error", "code: {0} (0x{1:X4})", err, (int) err);
    }

    public static void CheckVersion(this GL gl, uint major, uint minor)
    {
        var version = gl.GetStringS(StringName.Version);
        var endOfInfo = version.IndexOf(' ');
        if (endOfInfo != -1)
            version = version[..endOfInfo];

        var versionParts = version.Split('.').Select(int.Parse).ToArray();

        if (major > versionParts[0] || minor > versionParts[1])
            throw new GLException($"Version {major}.{minor} required (found {versionParts[0]}.{versionParts[1]})");
    }
    
    public static unsafe uint CompileChecked(this GL gl, ShaderType type, Span<byte> data)
    {
        uint res = gl.CreateShader(type);
        try
        {
            fixed (byte* pData = data)
            {
                gl.ShaderSource(res, 1, in pData, data.Length);
                gl.CompileShader(res);
            }
            if (gl.GetShader(res, ShaderParameterName.CompileStatus) == 0)
                throw new ShaderBuildException((ShaderBuildException.ObjectType) type, gl.GetShaderInfoLog(res));

            return res;
        }
        catch
        {
            gl.DeleteShader(res);
            throw;
        }
    }

    public static void LinkChecked(this GL gl, uint program)
    {
        gl.LinkProgram(program);
        if (gl.GetProgram(program, ProgramPropertyARB.LinkStatus) == 0)
            throw new ShaderBuildException(ShaderBuildException.ObjectType.Program, gl.GetProgramInfoLog(program));
    }

    public static void SetupAttribute(this VertexArray array, uint location, uint bindIndex, int count,
        VertexAttribType type, bool normalized, uint offset = 0)
    {
        array.AttributeEnable(location);
        array.AttributeBind(location, bindIndex);
        array.AttributeFormat(location, count, type, normalized, offset);
    }
    
    public static void SetupAttributeI(this VertexArray array, uint location, uint bindIndex, int count,
        VertexAttribIType type, uint offset = 0)
    {
        array.AttributeEnable(location);
        array.AttributeBind(location, bindIndex);
        array.AttributeIFormat(location, count, type, offset);
    }
}