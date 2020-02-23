using System;
using Crystal.Framework.UI;
using Crystal.Engine.Backends.MonoGame.Wrappers;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Backends.MonoGame
{
    public static class IFontExtensions
    {
        public static SpriteFont ToMonoGame(this IFont self)
        {
            if (self is SpriteFontWrapper spriteFontWrapper)
            {
                return spriteFontWrapper.Resource;
            }

            throw new Exception("Could not cast IFont to SpriteFont!");
        }
    }
}