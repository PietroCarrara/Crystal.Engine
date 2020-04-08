using Crystal.Framework;
using Crystal.Framework.Graphics;
using Crystal.Engine.Content;
using Crystal.Engine.Graphics;
using Crystal.Engine.Scene.Loaders;

namespace Crystal.Engine.Scene
{
    public class CrystalScene : Framework.Scene
    {
        private CrystalGame game;
        private SceneInitializer initializer;
        private ContentManager content;

        public CrystalScene(CrystalGame game,
                            SceneInitializer initializer,
                            ContentManager content,
                            IScaler scaler,
                            Point size)
        : base(size,
               content,
               new CrystalCanvas((uint)size.X, (uint)size.Y),
               new CrystalWindowCanvas(game.Window),
               scaler)
        {
            this.game = game;
            this.initializer = initializer;
            this.content = content;
        }

        public static Framework.Scene Loader(string path, ContentManager content)
        {
            var init = SceneLoader.FromFilePath(path);

            var scene = new CrystalScene(
                CrystalGame.Instance,
                init,
                content,
                init.GetScaler(),
                init.GetSize()
            );
            scene.Initialize();

            return scene;
        }

        public override void Initialize()
        {
            foreach (var system in initializer.GetSystems(this.content))
            {
                this.Add(system);
            }

            foreach (var renderer in initializer.GetRenderers(this.content))
            {
                this.Add(renderer);
            }

            foreach (var entity in initializer.GetEntities(this.content))
            {
                this.Add(entity);
            }

            foreach (var action in initializer.GetActions())
            {
                this.Actions.Add(action);
            }

            base.Initialize();
        }

        public override void Pop()
        {
            throw new System.NotImplementedException();
        }

        public override Framework.Scene Push(string name)
        {
            throw new System.NotImplementedException();
        }

        public override Framework.Scene Swap(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}