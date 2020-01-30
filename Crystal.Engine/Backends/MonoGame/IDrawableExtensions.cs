using System;
using Crystal.Framework.Graphics;
using Crystal.Framework.Components;
using Crystal.Engine.Graphics;
using Crystal.Engine.Backends.MonoGame.Wrappers;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Backends.MonoGame
{
    public static class IDrawableExtensions
    {
        public static Texture2D ToTexture2D(this IDrawable self)
        {
            if (self is RenderTarget2DWrapper r)
            {
                return r.Resource;
            }
            else if (self is Texture2DWrapper tex)
            {
                return tex.Resource;
            }
            else if (self is CrystalSceneViewport sceneViewport)
            {
                return sceneViewport.Texture;
            }
            else if (self is Sprite spr)
            {
                return spr.Texture.ToTexture2D();
            }
            else if (self is SpriteAnimation sprAnim)
            {
                return sprAnim.Animation.ToTexture2D();
            }
            else if (self is SpriteSheetAnimation sprShtAnim)
            {
                return sprShtAnim.Texture.ToTexture2D();
            }

            throw new Exception("Could not cast IDrawable to Texture2D!");
        }
    }
}