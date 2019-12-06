using System;
using Crystal.Framework.Graphics;
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
            else if (self is Texture2DWrapper s)
            {
                return s.Resource;
            }

            throw new Exception("Could not cast IDrawable to Texture2D!");
        }
    }
}