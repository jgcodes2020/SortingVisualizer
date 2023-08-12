using System.Collections;
using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using SortingVisualizer.Misc;

namespace SortingVisualizer.Rendering.OpenGL;

/// <summary>
/// Convenience class for specifying a list of shaders.
/// </summary>
public sealed class ShaderList : IEnumerable<KeyValuePair<ShaderType, Uri>>
{
    private readonly List<KeyValuePair<ShaderType, Uri>> _inner = new(8);

    public IEnumerator<KeyValuePair<ShaderType, Uri>> GetEnumerator()
    {
        return _inner.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(ShaderType type, Uri source)
    {
        _inner.Add(new KeyValuePair<ShaderType, Uri>(type, source));
    }

    public int Count => _inner.Count;
}

public sealed class ShaderProgram : IDisposable
{
    private GL _gl;
    private readonly uint _handle;

    public ShaderProgram(GL gl, IEnumerable<KeyValuePair<ShaderType, Uri>> shaders)
    {
        _gl = gl;
        
        var compiled = new List<uint>();
        foreach (var shader in shaders)
        {
            var data = shader.Value.ReadAllBytes();
            compiled.Add(_gl.CompileChecked(shader.Key, data));
        }

        _handle = _gl.CreateProgram();
        foreach (var shader in compiled)
        {
            _gl.AttachShader(_handle, shader);
        }
        
        try
        {
            _gl.LinkChecked(_handle);
        }
        finally
        {
            foreach (var shader in compiled)
            {
                _gl.DetachShader(_handle, shader);
                _gl.DeleteShader(shader);
            }
        }
    }

    public ShaderProgram(GL gl, ShaderList shaders) : this(gl, (IEnumerable<KeyValuePair<ShaderType, Uri>>) shaders)
    {
    }
    
    public void MakeCurrent()
    {
        _gl.UseProgram(_handle);
    }

    #region Scalar uniforms

    public void Uniform(int location, double value)
    {
        _gl.ProgramUniform1(_handle, location, value);
    }
    public void Uniform(int location, float value)
    {
        _gl.ProgramUniform1(_handle, location, value);
    }
    public void Uniform(int location, int value)
    {
        _gl.ProgramUniform1(_handle, location, value);
    }
    public void Uniform(int location, uint value)
    {
        _gl.ProgramUniform1(_handle, location, value);
    }

    #endregion

    #region Silk.NET.Vector*D<T>
    
    #region 2D uniforms

    public void Uniform(int location, Vector2D<double> value)
    {
        _gl.ProgramUniform2(_handle, location, value.X, value.Y);
    }
    public void Uniform(int location, Vector2D<float> value)
    {
        _gl.ProgramUniform2(_handle, location, value.X, value.Y);
    }
    public void Uniform(int location, Vector2D<int> value)
    {
        _gl.ProgramUniform2(_handle, location, value.X, value.Y);
    }
    public void Uniform(int location, Vector2D<uint> value)
    {
        _gl.ProgramUniform2(_handle, location, value.X, value.Y);
    }

    #endregion
    
    #region 3D uniforms

    public void Uniform(int location, Vector3D<double> value)
    {
        _gl.ProgramUniform3(_handle, location, value.X, value.Y, value.Z);
    }
    public void Uniform(int location, Vector3D<float> value)
    {
        _gl.ProgramUniform3(_handle, location, value.X, value.Y, value.Z);
    }
    public void Uniform(int location, Vector3D<int> value)
    {
        _gl.ProgramUniform3(_handle, location, value.X, value.Y, value.Z);
    }
    public void Uniform(int location, Vector3D<uint> value)
    {
        _gl.ProgramUniform3(_handle, location, value.X, value.Y, value.Z);
    }

    #endregion
    
    #region 4D uniforms

    public void Uniform(int location, Vector4D<double> value)
    {
        _gl.ProgramUniform4(_handle, location, value.X, value.Y, value.Z, value.W);
    }
    public void Uniform(int location, Vector4D<float> value)
    {
        _gl.ProgramUniform4(_handle, location, value.X, value.Y, value.Z, value.W);
    }
    public void Uniform(int location, Vector4D<int> value)
    {
        _gl.ProgramUniform4(_handle, location, value.X, value.Y, value.Z, value.W);
    }
    public void Uniform(int location, Vector4D<uint> value)
    {
        _gl.ProgramUniform4(_handle, location, value.X, value.Y, value.Z, value.W);
    }

    #endregion
    
    #endregion

    #region Silk.NET.Matrix*X*<T>

    #region 2-row matrix uniforms

    public unsafe void Uniform(int location, Matrix2X2<float> value)
    {
        _gl.ProgramUniformMatrix2(_handle, location, 1, true, (float*) &value);
    }

    public unsafe void Uniform(int location, Matrix2X2<double> value)
    {
        _gl.ProgramUniformMatrix2(_handle, location, 1, true, (double*) &value);
    }
    
    public unsafe void Uniform(int location, Matrix2X3<float> value)
    {
        _gl.ProgramUniformMatrix3x2(_handle, location, 1, true, (float*) &value);
    }

    public unsafe void Uniform(int location, Matrix2X3<double> value)
    {
        _gl.ProgramUniformMatrix3x2(_handle, location, 1, true, (double*) &value);
    }
    
    public unsafe void Uniform(int location, Matrix2X4<float> value)
    {
        _gl.ProgramUniformMatrix4x2(_handle, location, 1, true, (float*) &value);
    }

    public unsafe void Uniform(int location, Matrix2X4<double> value)
    {
        _gl.ProgramUniformMatrix4x2(_handle, location, 1, true, (double*) &value);
    }
    
    #endregion

    #region 3-row matrix uniforms

    public unsafe void Uniform(int location, Matrix3X2<float> value)
    {
        _gl.ProgramUniformMatrix2x3(_handle, location, 1, true, (float*) &value);
    }

    public unsafe void Uniform(int location, Matrix3X2<double> value)
    {
        _gl.ProgramUniformMatrix2x3(_handle, location, 1, true, (double*) &value);
    }
    
    public unsafe void Uniform(int location, Matrix3X3<float> value)
    {
        _gl.ProgramUniformMatrix3(_handle, location, 1, true, (float*) &value);
    }

    public unsafe void Uniform(int location, Matrix3X3<double> value)
    {
        _gl.ProgramUniformMatrix3(_handle, location, 1, true, (double*) &value);
    }
    
    public unsafe void Uniform(int location, Matrix3X4<float> value)
    {
        _gl.ProgramUniformMatrix4x3(_handle, location, 1, true, (float*) &value);
    }

    public unsafe void Uniform(int location, Matrix3X4<double> value)
    {
        _gl.ProgramUniformMatrix4x3(_handle, location, 1, true, (double*) &value);
    }

    #endregion
    
    #region 4-row matrix uniforms
    
    public unsafe void Uniform(int location, Matrix4X2<float> value)
    {
        _gl.ProgramUniformMatrix2x4(_handle, location, 1, true, (float*) &value);
    }

    public unsafe void Uniform(int location, Matrix4X2<double> value)
    {
        _gl.ProgramUniformMatrix2x4(_handle, location, 1, true, (double*) &value);
    }
    
    public unsafe void Uniform(int location, Matrix4X3<float> value)
    {
        _gl.ProgramUniformMatrix3x4(_handle, location, 1, true, (float*) &value);
    }

    public unsafe void Uniform(int location, Matrix4X3<double> value)
    {
        _gl.ProgramUniformMatrix3x4(_handle, location, 1, true, (double*) &value);
    }
    
    public unsafe void Uniform(int location, Matrix4X4<float> value)
    {
        _gl.ProgramUniformMatrix4(_handle, location, 1, true, (float*) &value);
    }

    public unsafe void Uniform(int location, Matrix4X4<double> value)
    {
        _gl.ProgramUniformMatrix4(_handle, location, 1, true, (double*) &value);
    }

#endregion

    #endregion

    #region System.Numerics

    public void Uniform(int location, Vector2 value)
    {
        _gl.ProgramUniform2(_handle, location, value);
    }
    
    public void Uniform(int location, Vector3 value)
    {
        _gl.ProgramUniform3(_handle, location, value);
    }
    
    public void Uniform(int location, Vector4 value)
    {
        _gl.ProgramUniform4(_handle, location, value);
    }

    public unsafe void Uniform(int location, Matrix3x2 value)
    {
        _gl.ProgramUniformMatrix3x2(_handle, location, 1, true, (float*) &value);
    }

    public unsafe void Uniform(int location, Matrix4x4 value)
    {
        _gl.ProgramUniformMatrix4(_handle, location, 1, true, (float*) &value);
    }

    #endregion

    public void Dispose()
    {
        _gl.DeleteShader(_handle);
    }
}