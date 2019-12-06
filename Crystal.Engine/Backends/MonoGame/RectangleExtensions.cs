using System;
using Crystal.Framework.Graphics;
using Microsoft.Xna.Framework;
using Rect = Crystal.Framework.Rectangle;

namespace Crystal.Engine.Backends.MonoGame
{
    public static class RectangleExtensions
    {
        public static Rectangle ToMonoGame(this TextureSlice self)
        {
            return new Rectangle(self.TopLeft.X, self.TopLeft.Y, self.Width, self.Height);
        }

        public static Rectangle ToMonoGame(this Rect self)
        {
            return new Rectangle(
                (int)self.Position.X,
                (int)self.Position.Y,
                (int)self.Width,
                (int)self.Height
            );
        }

        public static TextureSlice ToTextureSlice(this Rectangle self)
        {
            return new TextureSlice(self.X, self.Y, self.Width, self.Height);
        }

        public static Rect ToCrystal(this Rectangle self)
        {
            return new Rect(
                self.Left,
                self.Top,
                self.Width,
                self.Height
            );
        }
    }
}