using OpenTK.Graphics.OpenGL4;

namespace OpenTK_Gaem;
public class Shader
{
    int Handle; 
    int VertexShader;
    int FragmentShader;

    private int success;

    public Shader(string vertexPath, string fragmentPath)
    {
        // reading the shader text
        string vertexShaderSource = File.ReadAllText(vertexPath);
        string fragmentShaderSource = File.ReadAllText(fragmentPath);
        
        // generating shader and binding the source code to the shader
        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(VertexShader, vertexShaderSource);

        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(FragmentShader, fragmentShaderSource);
        
        // compiling shader
        
        GL.CompileShader(VertexShader);
        GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(VertexShader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(FragmentShader);
        GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            string infoLog = GL.GetShaderInfoLog(FragmentShader);
            Console.WriteLine(infoLog);
        }
        
        // linking shader
        
        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(infoLog);
        }
        
        // delete source of the shader
        
        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(FragmentShader);
        GL.DeleteShader(VertexShader);
    }
    
    public void Use() // use the shader
    {
        GL.UseProgram(Handle);
    }
    
    // clean up the handle
    
    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(Handle);

            disposedValue = true;
        }
    }

    ~Shader()
    {
        if (disposedValue == false)
        {
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}