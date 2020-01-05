using System;
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

        public Stack<Scene> Scenes { get; private set; } = new Stack<Scene>();

        public CrystalGame(CrystalConfig config)
        {
            this.Config = config;

            this.MainScene = this.Config.MainScene;

            this.Scenes.Push(
                new CrystalScene(this.MainScene, this)
            );

            this.Window.Title = config.Project;
            this.Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var scene = Scenes.Peek();

            scene.Load();
        }

        public override void Update(float delta)
        {
            Scene scene;

            if (Scenes.TryPeek(out scene))
            {
                scene.Update(delta);
            }
            else
            {
                this.Exit();
            }
        }

        public override void Render(SpriteBatch sp, float delta)
        {
#if DEBUG
            this.Window.Title = $"{this.Config.Project} [{Math.Floor(1 / delta)} FPS]";
#endif

            Scene scene;

            if (Scenes.TryPeek(out scene))
            {
                scene.Render(delta);

                this.GraphicsDevice.Clear(Color.Black);
                var tex = scene.Viewport.ToTexture2D();

                // Letterboxing
                Rectangle targetRect = new Rectangle();
                (_, _, targetRect.Width, targetRect.Height) = this.GraphicsDevice.Viewport.Bounds;

                var scaleY = tex.Width / (float)targetRect.Width;
                var scaleX = tex.Height / (float)targetRect.Height;
                var scale = Math.Max(scaleX, scaleY);

                var width = (int)(tex.Width / scale);
                var height = (int)(tex.Height / scale);
                
                targetRect = new Rectangle((targetRect.Width - width) / 2,
                                           (targetRect.Height - height) / 2,
                                           width,
                                           height);

                sp.Begin();
                sp.Draw(
                    tex,
                    targetRect,
                    null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0);
                sp.End();
            }
        }
    }
}