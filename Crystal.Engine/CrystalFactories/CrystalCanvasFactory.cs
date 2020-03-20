using Crystal.Framework;
using Crystal.Framework.Graphics;
using Crystal.Framework.LowLevel;
using Crystal.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Factories
{
    public class CrystalCanvasFactory : CanvasFactory
    {
        private GraphicsDevice graphics;
        private GameWindow window;
        
        public CrystalCanvasFactory(GraphicsDevice graphics, GameWindow window)
        {
            this.graphics = graphics;
            this.window = window;
        }
        
        public override Canvas Create()
        {
            return new ResizingCrystalCanvas(graphics, window);
        }

        public override Canvas Create(Crystal.Framework.Point size)
        {
            var canvas = new CrystalCanvas(graphics);
            canvas.SetSize(size);
            
            return canvas;
        }
    }
}