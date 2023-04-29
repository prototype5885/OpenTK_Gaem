using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Reflection.Metadata;
using StbImageSharp;

using System;
using System.IO;
public class Game : GameWindow
{
    Shader shader;
    int VertexBufferObject;
    int VertexArrayObject;
    int ElementBufferObject;

    float[] vertices =
    {
        //Position          Texture coordinates
         0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
         0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
    };

    uint[] indices = // note that we start from 0!
    {
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
    };


    public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        KeyboardState input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }

    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        VertexBufferObject = GL.GenBuffer();
        VertexArrayObject = GL.GenVertexArray();
        ElementBufferObject = GL.GenBuffer();

        GL.BindVertexArray(VertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);


        //GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        //int texCoordLocation = shader.GetAttribLocation("aTexCoord");
        //GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        GL.EnableVertexAttribArray(0);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        shader.Use();

        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

        

        SwapBuffers();
    }


    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }


    protected override void OnUnload()
    {
        base.OnUnload();

        shader.Dispose();
    }



    public static void Main()
    {
        using (Game game = new Game(1080, 1080, "Gaem"))
        {
            game.Run();
        }
    }
}

public class Texture
{
    int Handle;
    public Texture()
    {
        Handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, Handle);
        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageResult image = ImageResult.FromStream(File.OpenRead("texture.png"), ColorComponents.RedGreenBlueAlpha);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
    }

    //public void Use()
    //{
    //    GL.UseProgram(Handle);
    //}

}
