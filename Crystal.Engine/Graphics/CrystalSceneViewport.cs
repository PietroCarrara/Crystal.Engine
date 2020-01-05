using Crystal.Framework;
using Crystal.Framework.ECS;
using Crystal.Framework.Math;
using Crystal.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Graphics
{
    public class CrystalSceneViewport : SceneViewport
    {
        public RenderTarget2D Texture { get; private set; }

        private readonly GraphicsDevice graphicsDevice;

        public override int Width => Texture.Width;

        public override int Height => Texture.Height;

        public CrystalSceneViewport(GraphicsDevice graphicsDevice, Point size, Scene scene) : base(scene)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public override void SetSize(Point size)
        {
            base.SetSize(size);
            
            this.Texture?.Dispose();
            this.Texture = new RenderTarget2D(this.graphicsDevice, size.X, size.Y);
        }

        public override void Dispose()
        {
            this.Texture.Dispose();
        }

    }
}