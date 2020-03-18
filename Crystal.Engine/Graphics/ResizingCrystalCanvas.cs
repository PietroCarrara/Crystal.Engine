using System;
using Crystal.Framework;
using Crystal.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Graphics
{
    /// <summary>
    /// A render target abstraction that automatically resizes whenever the window does
    /// </summary>
    public class ResizingCrystalCanvas : CrystalCanvas
    {
        private GameWindow window;

        public ResizingCrystalCanvas(GraphicsDevice gd, GameWindow window) : base(gd)
        {
            this.window = window;
            window.ClientSizeChanged += resize;

            this.resize();
        }

        private void resize(object sender = null, EventArgs e = null)
        {
            this.SetSize(
                (
                    graphicsDevice.PresentationParameters.BackBufferHeight,
                    graphicsDevice.PresentationParameters.BackBufferWidth
                )
            );
        }

        public override void Dispose()
        {
            base.Dispose();
            window.ClientSizeChanged -= this.resize;
        }
    }
}