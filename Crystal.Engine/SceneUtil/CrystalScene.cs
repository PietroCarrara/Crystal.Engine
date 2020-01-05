using Crystal.Framework.ECS;
using Crystal.Engine.Backends.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.SceneUtil
{
    public class CrystalScene : Scene
    {
        CrystalGame game;
        
        public CrystalScene(string name, CrystalGame game) : base(name)
        {
            this.game = game;
        }

        public override Scene Push(string name)
        {
            var scene = new CrystalScene(name, this.game);
            
            this.game.Scenes.Push(scene);
            scene.Load();

            return scene;
        }

        public override Scene Swap(string name)
        {
            this.Pop();
            return this.Push(name);
        }

        public override void Pop()
        {
            this.Unload();
            this.game.Scenes.Pop();
        }

        public override void AfterRender()
        {
            base.AfterRender();

            this.game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void BeforeRender()
        {
            base.BeforeRender();

            this.game.GraphicsDevice.SetRenderTarget((RenderTarget2D)this.Viewport.ToTexture2D());
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