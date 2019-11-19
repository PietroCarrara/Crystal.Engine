using Crystal.Engine.Config;
using System.Collections.Generic;
using Crystal.Framework.ECS;
using Crystal.Engine.SceneUtil;
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
                new Scene(this.MainScene)
            );
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
            var scene = scenes.Peek();
               
            SceneInitializer.Initializers[scene.Name].Initialize(
                this,
                scene,
                this.Content
            );
        } 

        public override void Update(float delta)
        {
            var scene = scenes.Peek();

            scene.Input.Update();

            foreach (var system in scene.Systems)
            {
                system.Update(scene, delta);
            }
        }

        public override void Render(SpriteBatch sp)
        {
            var scene = scenes.Peek();

            foreach (var renderer in scene.Renderers)
            {
                renderer.Render(scene);
            }
        }
    }
}