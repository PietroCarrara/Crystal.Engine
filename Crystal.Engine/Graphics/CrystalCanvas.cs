using Crystal.Framework;
using Crystal.Framework.Graphics;
using Crystal.Engine.SFML;
using SFML.Graphics;
using SFML.Window;

namespace Crystal.Engine.Graphics
{
    public class CrystalCanvas : IResizeableRenderTarget
    {
        private RenderTexture RenderTexture;
        private Point maximumSize, size;

        public Point Size => size;

        public event IRenderTarget.SizeChangedEventHandler SizeChanged;

        public CrystalCanvas(uint width, uint height)
        {
            this.RenderTexture = new RenderTexture(width, height);

            this.size = this.maximumSize = new Point((int)width, (int)height);
        }

        public void Clear(Framework.Color color)
        {
            this.RenderTexture.Clear(color.ToSFML());
        }

        public void Dispose()
        {
            this.RenderTexture.Dispose();
        }

        public void SetSize(Point size)
        {
            var resize = false;

            if (this.maximumSize.X < size.X)
            {
                resize = true;
                this.maximumSize.X = size.X;
            }

            if (this.maximumSize.Y < size.Y)
            {
                resize = true;
                this.maximumSize.Y = size.Y;
            }

            if (resize)
            {
                this.RenderTexture.Dispose();
                this.RenderTexture = new RenderTexture((uint)maximumSize.X, (uint)maximumSize.Y);
            }

            this.size = size;

            SizeChanged?.Invoke(this, size);
        }
    }
}