using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace OpenTK_Gaem;

public class Texture
{
    int Handle;

    public Texture()
    {
        Handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, Handle);
        StbImage.stbi_set_flip_vertically_on_load(1);
        var image = ImageResult.FromStream(File.OpenRead("texture.png"), ColorComponents.RedGreenBlueAlpha);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
            PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }
}