using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace OpenTK_Gaem;

public class Game : GameWindow
{
    private Stopwatch fpsTimer = new Stopwatch();
    private int fpsCounter = 0;
    private double fps = 0;

    
    int VertexBufferObject;
    int VertexArrayObject;

    Shader shader;
    
    
    float[] vertices = {
        -0.5f, -0.5f, 0.0f, //Bottom-left vertex
        0.5f, -0.5f, 0.0f, //Bottom-right vertex
        0.0f,  0.5f, 0.0f  //Top vertex
    };

    public Game(int width, int height, string title) : 
        base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = (width, height), Title = title
        }) { }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);
        
        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        
        shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        
        VertexArrayObject = GL.GenVertexArray();
        
        // 1. bind Vertex Array Object
        GL.BindVertexArray(VertexArrayObject); 
        
        // 2. copy our vertices array in a buffer for OpenGL to use
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        // 3. then set our vertex attributes pointers
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        fpsTimer.Start();
    }
    
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();
        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        
        SwapBuffers();
        
        // Calculate FPS
        fpsCounter++;
        if (fpsTimer.ElapsedMilliseconds >= 1000)
        {
 
            fps = fpsCounter / (fpsTimer.Elapsed.TotalSeconds);
            Title = $"FPS: {fps:F2}";

            fpsTimer.Restart();
            fpsCounter = 0;
        }
    }
    
    protected override void OnUnload()
    {
        base.OnUnload();

        shader.Dispose();
    }
    
    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        // KeyboardState input = KeyboardState;

        if (KeyboardState.IsKeyDown(Keys.Escape)) // was input instead of KeyboardState first
        {
            Close();
        }

    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}