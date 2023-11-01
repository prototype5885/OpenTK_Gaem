using LearnOpenTK.Common;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace OpenTK_Gaem;

public class Game : GameWindow
{
    Stopwatch fpsTimer = new Stopwatch();
    int fpsCounter = 0;
    double fps = 0;

    //Stopwatch shaderTimer = new Stopwatch();

    int VertexBufferObject;
    int VertexArrayObject;
    int TextureVBO;
    int ElementBufferObject;

    Shader shader;

    private Texture _texture;

    //List<Vector3> vertices = new List<Vector3>()
    //{
    //    new Vector3(0f, 1f, 0f), // top right
    //    new Vector3(0f, 0f, 0f), // bottom right
    //    new Vector3(-1f, 0f, 0f), // bottom left
    //    new Vector3(-1f, 1f, 0f) // top left


    float[] tile =
    {
        1f, 1f, 0f, // top right
        1f, -1f, 0f, // bottom right
        -1f, -1f, 0f, // bottom left
        -1f, 1f, 0f // top left
    };


    float[] vertices;

    List<Vector2> texCoordinates = new List<Vector2>()
    {
        new Vector2(1f, 1f),
        new Vector2(1f, 0f),
        new Vector2(0f, 0f),
        new Vector2(0f, 1f),
    };


    uint[] indices =
    {  
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    };

    float currentRotation;

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

        vertices = new float[12];

        for (int i = 0; i < tile.Length; i++)
        {
            //tile[i] = tile[i] / 2;
            vertices[i] = tile[i];
        }

        //for (int v = 0; v < vertices.Length; v++)
        //{
        //    Console.Write(vertices[v]);
        //    vertices[v] = vertices[v] + 0.2f;
        //}

        GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);

        // shader
        shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        shader.Use();

        // 1. VAO
        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        // 2. VBO
        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        //GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        var vertexLocation = shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        //GL.EnableVertexArrayAttrib(VertexArrayObject, 0);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 0, 0);

        // Texture VBO
        TextureVBO = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, TextureVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, texCoordinates.Count * Vector2.SizeInBytes, texCoordinates.ToArray(), BufferUsageHint.StaticDraw);
        //GL.BufferData(BufferTarget.ArrayBuffer, texCoordinates.Length * sizeof(float), texCoordinates, BufferUsageHint.StaticDraw);

        var texCoordLocation = shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        //GL.EnableVertexArrayAttrib(VertexArrayObject, 1);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 0, 0);

        // 3. EBO
        ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);


        _texture = Texture.LoadFromFile("textures/texture.png");
        _texture.Use(TextureUnit.Texture0);




        fpsTimer.Start();
        //shaderTimer.Start();

    }


    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);


        GL.BindVertexArray(VertexArrayObject);
        var transform = Matrix4.Identity;
        currentRotation += 2;
        transform = transform * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(currentRotation));
        transform = transform * Matrix4.CreateScale(0.5f);


        transform = transform * Matrix4.CreateTranslation(0f, 0f, 0f);
        shader.Use();
        shader.SetMatrix4("transform", transform);


        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);



        GL.BindVertexArray(VertexArrayObject);


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