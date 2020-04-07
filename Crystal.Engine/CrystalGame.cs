using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Crystal.Engine.Config;
using Crystal.Engine.Input;
using Crystal.Engine.Content;

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
            var clock = new Clock();
            var input = new CrystalInput();
            var content = new ContentManager();

            this.Window = new RenderWindow(new VideoMode(800, 600), config.Project);

            this.PushScene(content.Load<Framework.Scene>(config.MainScene));

            while (Window.IsOpen)
            {
                Window.DispatchEvents();

                var delta = clock.Restart().AsSeconds();

                this.update(delta, input);
                this.render(delta);

                Window.Display();
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

        private void render(float delta)
        {
            if (scenes.TryPeek(out var scene))
            {
                // TODO: Render
                // scene.Render(delta, drawer);
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