using Crystal.Framework.Graphics;
using Crystal.Engine.Graphics;
using SFML.Graphics;

namespace Crystal.Engine.SFML
{
    public static class DrawableExtensions
    {
        public static Texture ToSFML(this IDrawable self)
        {
            if (self is SpriteSheetAnimation spriteSheetAnimation)
            {
                return spriteSheetAnimation.Texture.ToSFML();
            }

            return ((CrystalTexture)self).Texture;
        }
    }
}