using Crystal.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Backends.MonoGame.Wrappers
{
    public class Texture2DWrapper : MonoGameWrapper<Texture2D>, IDrawable
    {
        public int Width => this.Resource.Width;

        public int Height => this.Resource.Height;

        public Texture2DWrapper(Texture2D tex) : base(tex)
        { }

        public void Dispose()
        {
            this.Resource.Dispose();
        }
    }
}