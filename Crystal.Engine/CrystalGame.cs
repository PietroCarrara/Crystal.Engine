using System;
using Crystal.Engine.Config;
using System.Collections.Generic;
using Crystal.Framework;
using Crystal.Engine.Graphics;
using Crystal.Engine.SceneUtil;
using Crystal.Engine.Backends.MonoGame;
using Crystal.Engine.Backends.MonoGame.Wrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameVector2 = Microsoft.Xna.Framework.Vector2;

namespace Crystal.Engine
{
    public sealed class CrystalGame : BaseGame
    {
        public readonly CrystalConfig Config;
        public Stack<Scene> Scenes { get; private set; } = new Stack<Scene>();
        public ICrystalScaler Scaler { get; private set; }

        public new CrystalContentManager Content;

        private readonly string MainScene;

        public CrystalGame(CrystalConfig config)
        {
            this.Config = config;

            this.MainScene = this.Config.MainScene;

            this.Scenes.Push(
                new CrystalScene(this.MainScene, this)
            );

            this.Content = new CrystalContentManager(base.Content);

            this.Scaler = CrystalScalerFactory.FromStrategy(config.ScaleStrategy, this);

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

                var targetRect = this.Scaler.Scale(tex.Width, tex.Height);

                sp.Begin();
                sp.Draw(
                    tex,
                    targetRect,
                    null,
                    Color.White,
                    0,
                    MonogameVector2.Zero,
                    SpriteEffects.None,
                    0);
                sp.End();
            }
        }
    }
}