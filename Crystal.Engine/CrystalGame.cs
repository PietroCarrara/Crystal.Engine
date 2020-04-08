using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Crystal.Engine.SFML;
using Crystal.Engine.Config;
using Crystal.Engine.Input;
using Crystal.Engine.Content;
using Crystal.Engine.Graphics;

namespace Crystal.Engine
{
    public class CrystalGame
    {
        public static CrystalGame Instance { get; private set; }

        private CrystalConfig config;
        private Stack<Framework.Scene> scenes = new Stack<Framework.Scene>();

        public RenderWindow Window { get; private set; }

        public CrystalGame(CrystalConfig config)
        {
            if (Instance != null)
            {
                throw new Exception("Can't create more than one game object!");
            }

            Instance = this;
            this.config = config;
        }

        public void Run()
        {
            this.Window = new RenderWindow(new VideoMode(800, 600), config.Project);

            var clock = new Clock();
            var input = new CrystalInput(this.Window);
            var renderer = new CrystalRenderer();
            var content = new ContentManager();

            this.PushScene(content.Load<Framework.Scene>(config.MainScene));

            while (Window.IsOpen)
            {
                Window.DispatchEvents();

                var delta = clock.Restart().AsSeconds();

                this.update(delta, input);
                this.render(delta, renderer);

                input.AdvanceState();
            }
        }

        public void PushScene(Framework.Scene scene)
        {
            this.scenes.Push(scene);
        }

        public void PopScene()
        {
            if (this.scenes.TryPop(out var scene))
            {
                scene.Dispose();
            }
            else
            {
                this.close();
            }
        }

        private void update(float delta, CrystalInput input)
        {
            if (scenes.TryPeek(out var scene))
            {
                scene.Update(delta, input);
            }
            else
            {
                Window.Close();
            }
        }

        private void render(float delta, CrystalRenderer drawer)
        {
            Window.Clear();

            if (scenes.TryPeek(out var scene))
            {
                scene.Render(delta, drawer);

                var canvas = new Sprite(scene.Canvas.ToSFML().Texture);
                var windowCanvas = new Sprite(scene.WindowCanvas.ToSFML().Texture);

                Window.Draw(canvas);
                Window.Draw(windowCanvas);

                Window.Display();
            }
            else
            {
                Window.Close();
            }
        }

        private void close()
        {
            Window.Close();
        }
    }
}