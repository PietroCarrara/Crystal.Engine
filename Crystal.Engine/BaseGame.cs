using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Crystal.Engine
{
    public abstract class BaseGame : Game
    {
        // HACK: This is not at all a good
        // way to share the spritebatch
        public static SpriteBatch SpriteBatch { get; private set; }
        
        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public abstract void Update(float delta);
        public abstract void Render(SpriteBatch sp);

        public BaseGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BaseGame.SpriteBatch = spriteBatch;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            this.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected sealed override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            this.Render(this.spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
