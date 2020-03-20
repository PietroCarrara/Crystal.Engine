using System;
using Crystal.Framework.Graphics;
using Crystal.Framework.Math;
using Microsoft.Xna.Framework;
namespace Crystal.Engine.Graphics.Scalers
{
    public class LetterboxingScaler : IScaler
    {
        private readonly CrystalGame game;

        public LetterboxingScaler(CrystalGame g)
        {
            this.game = g;
        }

        public Matrix4 Invert(TextureSlice texture)
        {
            var screenSize = new Point();
            (_, _, screenSize.X, screenSize.Y) = this.game.GraphicsDevice.Viewport.Bounds;
            
            var scaled = this.Scale(texture);
            
            var matrix = Matrix4.CreateTranslation(-scaled.TopLeft.X, -scaled.TopLeft.Y, 0);

            return matrix;
        }

        public TextureSlice Scale(TextureSlice texture)
        {
            var screenSize = new Point();
            (_, _, screenSize.X, screenSize.Y) = this.game.GraphicsDevice.Viewport.Bounds;

            var scaleX = texture.Size.X / (float)screenSize.X;
            var scaleY = texture.Size.Y / (float)screenSize.Y;
            var scale = Math.Max(scaleY, scaleX);

            var width = (int)(texture.Size.X / scale);
            var height = (int)(texture.Size.Y / scale);

            return new TextureSlice(
                (screenSize.X - width) / 2,
                (screenSize.Y - height) / 2,
                width,
                height
            );
        }
    }
}