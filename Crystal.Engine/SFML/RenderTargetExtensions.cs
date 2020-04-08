using Crystal.Framework.Graphics;
using Crystal.Engine.Graphics;
using SFML.Graphics;

namespace Crystal.Engine.SFML
{
    public static class RenderTargetExtensions
    {
        public static RenderTexture ToSFML(this IRenderTarget self)
        {
            return ((CrystalCanvas)self).RenderTexture;
        }
    }
}