using Crystal.Engine.Config;
using System.Collections.Generic;
using Crystal.Framework.ECS;
using Crystal.Engine.SceneUtil;
using Crystal.Engine.Backends.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine
{
    public sealed class CrystalGame : BaseGame
    {
        public readonly CrystalConfig Config;
        private readonly string MainScene;

        private Stack<Scene> scenes = new Stack<Scene>();

        public CrystalGame(CrystalConfig config)
        {
            this.Config = config;

            this.MainScene = this.Config.MainScene;

            this.scenes.Push(
                new CrystalScene(this.MainScene, this)
            );
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
            var scene = scenes.Peek();
               
            scene.Load();
        } 

        public override void Update(float delta)
        {
            var scene = scenes.Peek();

            scene.Update(delta);
        }

        public override void Render(SpriteBatch sp)
        {
            var scene = scenes.Peek();
            scene.Render();

            // TODO: Distortionless draw
            sp.Begin();
            sp.Draw(
                scene.Viewport.ToTexture2D(),
                this.GraphicsDevice.Viewport.Bounds,
                null,
                Color.White,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                0
            );
            sp.End();
        }
    }
}