using Crystal.Framework;
using Crystal.Framework.Graphics;
using SFML.Window;

namespace Crystal.Engine.Graphics
{
    public class CrystalWindowCanvas : CrystalCanvas
    {
        private Window window;

        public CrystalWindowCanvas(Window window) : base(window.Size.X, window.Size.Y)
        {
            this.window = window;

            window.Resized += onResize;
        }

        private void onResize(object sender, SizeEventArgs args)
        {
            this.SetSize(((int)args.Width, (int)args.Height));
        }
    }
}