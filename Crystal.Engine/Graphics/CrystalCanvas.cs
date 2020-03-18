using Crystal.Framework;
using Crystal.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Graphics
{
    /// <summary>
    /// A render target abstraction
    /// </summary>
    public class CrystalCanvas : Canvas
    {
        protected GraphicsDevice graphicsDevice { get; private set; }
        private RenderTarget2D renderTarget;
        
        public CrystalCanvas(GraphicsDevice gd)
        {
            this.graphicsDevice = gd;
        }
        
        public override void SetSize(Point size)
        {
            if (renderTarget != null)
            {
                renderTarget.Dispose();
            }

            this.renderTarget = new RenderTarget2D(graphicsDevice, size.X, size.Y);
        }

        public override void Dispose()
        {
            this.renderTarget.Dispose();
        }
    }
}