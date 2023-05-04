using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace OpenTK_Gaem;

public class Game : GameWindow
{
    Stopwatch fpsTimer = new Stopwatch();
    int fpsCounter = 0;
    double fps = 0;

    //Stopwatch shaderTimer = new Stopwatch();

    int VertexBufferObject;
    int VertexArrayObject;
    int ElementBufferObject;

    Shader shader;

    private Texture _texture;

    float[] vertices =
    {
        //Position          Texture coordinates
        0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
    };

    uint[] indices =
    {  
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    };


    public Game(int width, int height, string title) :
        base(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = (width, height),
            Title = title
        })
    { }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);




        // 1. VAO
        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        // 2. VBO
        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        // 3. EBO
        ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        // shader
        shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        shader.Use();

        // 4. then set our vertex attributes pointers
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // Because there's now 5 floats between the start of the first vertex and the start of the second,
        // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
        // This will now pass the new vertex array to the buffer.
        var vertexLocation = shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        // Next, we also setup texture coordinates. It works in much the same way.
        // We add an offset of 3, since the texture coordinates comes after the position data.
        // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
        var texCoordLocation = shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        _texture = Texture.LoadFromFile("textures/texture.png");
        _texture.Use(TextureUnit.Texture0);

        //Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90.0f));
        //Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);
        //Matrix4 trans = rotation * scale;

        //GL.UseProgram(program);

        //int location = GL.GetUniformLocation(Handle, name);

        //GL.UniformMatrix4(location, true, ref matrix);


        fpsTimer.Start();
        //shaderTimer.Start();

    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        

        //double timeValue = shaderTimer.Elapsed.TotalSeconds;
        //float greenValue = (float)Math.Sin(timeValue) / 2.0f + 0.5f;
        //int vertexColorLocation = GL.GetUniformLocation(shader.Handle, "ourColor");
        //GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);


        GL.BindVertexArray(VertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();

        calculateFPS();
    }

    private void calculateFPS()
    {
        //Calculate FPS
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

        //shader.Dispose();
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