using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Crystal.Engine
{
    public abstract class BaseGame : Game
    {
        public SpriteBatch SpriteBatch { get; private set; }
        
        GraphicsDeviceManager graphics;

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
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            this.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected sealed override void Draw(GameTime gameTime)
        {
            this.Render(this.SpriteBatch);

            base.Draw(gameTime);
        }
    }
}
