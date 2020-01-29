using System;
using Microsoft.Xna.Framework;

namespace Crystal.Engine.Graphics.Scalers
{
    public class LetterboxingScaler : ICrystalScaler
    {
        private readonly CrystalGame game;

        public LetterboxingScaler(CrystalGame g)
        {
            this.game = g;
        }
        
        public Rectangle Scale(int w, int h)
        {
            Rectangle targetRect = new Rectangle();
            (_, _, targetRect.Width, targetRect.Height) = this.game.GraphicsDevice.Viewport.Bounds;

            var scaleY = w / (float)targetRect.Width;
            var scaleX = h / (float)targetRect.Height;
            var scale = Math.Max(scaleX, scaleY);

            var width = (int)(w / scale);
            var height = (int)(h / scale);

            return new Rectangle((targetRect.Width - width) / 2,
                                 (targetRect.Height - height) / 2,
                                 width,
                                 height);


        }
    }
}