using System;
using System.Collections.Generic;
using Crystal.ECS;
using Crystal.Engine.SceneUtil;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine
{
    public sealed class CrystalGame : BaseGame
    {
        private readonly string MainScene;

        private Stack<Scene> scenes = new Stack<Scene>();

        public CrystalGame(string mainScene)
        {
            this.MainScene = mainScene;

            this.scenes.Push(
                new Scene(this.MainScene)
            );
        }

        public override void Update(float delta)
        {
            var scene = scenes.Peek();

            if (!scene.Initialized)
            {
                SceneInitializer.Initializers[scene.Name].Initialize(
                    scene,
                    this.Content
                );
            }

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