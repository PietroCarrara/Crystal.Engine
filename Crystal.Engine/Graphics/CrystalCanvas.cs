using Crystal.Framework;
using Crystal.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Graphics
{
    /// <summary>
    /// A render target abstraction
    /// </summary>
    public class CrystalCanvas : Canvas
    {
        public RenderTarget2D RenderTarget { get; private set; }
        protected GraphicsDevice graphicsDevice { get; private set; }
        
        public CrystalCanvas(GraphicsDevice gd)
        {
            this.graphicsDevice = gd;
        }
        
        public override void SetSize(Point size)
        {
            base.SetSize(size);
            
            if (RenderTarget != null)
            {
                RenderTarget.Dispose();
            }

            this.RenderTarget = new RenderTarget2D(
                graphicsDevice,
                size.X,
                size.Y,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents
            );
        }

        public override void Dispose()
        {
            this.RenderTarget.Dispose();
        }

        public override void Clear()
        {
            var rt = graphicsDevice.GetRenderTargets();
            graphicsDevice.SetRenderTarget(this.RenderTarget);
            graphicsDevice.Clear(Color.Transparent);
            graphicsDevice.SetRenderTargets(rt);
        }
    }
}