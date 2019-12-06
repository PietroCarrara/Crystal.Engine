using Crystal.Framework.ECS;
using Crystal.Engine.Backends.MonoGame.Wrappers;
using Microsoft.Xna.Framework;

namespace Crystal.Engine.SceneUtil
{
    public class CrystalScene : Scene
    {
        CrystalGame game;
        
        public CrystalScene(string name, CrystalGame game) : base(name)
        {
            this.game = game;
        }

        public override void AfterRender()
        {
            base.AfterRender();

            this.game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void BeforeRender()
        {
            base.BeforeRender();

            this.game.GraphicsDevice.SetRenderTarget(((RenderTarget2DWrapper)this.Viewport).Resource);
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        public override void Load()
        {
            base.Load();
            
            SceneInitializer.Initializers[this.Name].Initialize(
                game,
                this
            );
        }

        public override void Unload()
        {
            base.Unload();

            this.Viewport.Dispose();
        }
    }
}