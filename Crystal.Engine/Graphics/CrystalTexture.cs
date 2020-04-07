using SFML.Graphics;
using Crystal.Framework.Graphics;
using Crystal.Engine.Content;

namespace Crystal.Engine.Graphics
{
    public class CrystalTexture : IDrawable
    {
        public readonly Texture Texture;

        public int Width => (int)Texture.Size.X;

        public int Height => (int)Texture.Size.Y;

        public static IDrawable Loader(string path, ContentManager _)
        {
            return new CrystalTexture(new Texture(path));
        }

        public CrystalTexture(Texture texture)
        {
            this.Texture = texture;
        }

        public void Dispose()
        {
            Texture.Dispose();
        }
    }
}