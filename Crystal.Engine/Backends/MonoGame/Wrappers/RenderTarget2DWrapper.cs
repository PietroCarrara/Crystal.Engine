using Crystal.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Backends.MonoGame.Wrappers
{
    public class RenderTarget2DWrapper : MonoGameWrapper<RenderTarget2D>, IDrawable
    {
        public RenderTarget2DWrapper(RenderTarget2D content) : base(content)
        {
        }

        public int Width => this.Resource.Width;

        public int Height => this.Resource.Height;

        public void Dispose()
        {
            this.Resource.Dispose();
        }
    }
}