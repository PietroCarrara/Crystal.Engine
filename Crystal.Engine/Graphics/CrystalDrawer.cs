using Crystal.Framework;
using Crystal.Framework.Graphics;
using Crystal.Framework.Math;
using Crystal.Engine.Backends.MonoGame;
using Microsoft.Xna.Framework.Graphics;
using CrystalSampler = Crystal.Framework.Graphics.SamplerState;

namespace Crystal.Engine.Graphics
{
    public class CrystalDrawer : IDrawer
    {
        private CrystalGame game;
        private SpriteBatch spriteBatch;

        private Viewport? oldViewport;

        public CrystalDrawer(CrystalGame game)
        {
            this.game = game;
            this.spriteBatch = game.SpriteBatch;
        }

        public void BeginDraw(
            TextureSlice? viewport = null,
            Matrix4 transformMatrix = null,
            CrystalSampler samplerState = CrystalSampler.LinearClamp)
        {
            if (viewport.HasValue)
            {
                this.oldViewport = game.GraphicsDevice.Viewport;
                this.game.GraphicsDevice.Viewport = new Viewport(viewport.Value.ToMonoGame());
            }

            var matrix = transformMatrix?.ToMonoGame();

            this.spriteBatch.Begin(transformMatrix: matrix, samplerState: samplerState.ToMonogame());
        }

        public void Draw(IDrawable texture,
                         Vector2 position,
                         float delta,
                         Vector2? origin = null,
                         float rotation = 0,
                         Vector2? scale = null,
                         TextureSlice? sourceRectangle = null)
        {
            var tex = texture.ToTexture2D();

            var orig = origin.HasValue
                                ? origin.Value
                                : new Vector2(0.5f, 0.5f);
            orig *= new Vector2(texture.Width, texture.Height);

            this.spriteBatch.Draw(
                tex,
                position.ToMonoGame(),
                sourceRectangle?.ToMonoGame(),
                Microsoft.Xna.Framework.Color.White,
                rotation,
                orig.ToMonoGame(),
                scale.HasValue ? scale.Value.ToMonoGame() : Vector2.One.ToMonoGame(),
                SpriteEffects.None,
                0
            );
        }

        public void EndDraw()
        {
            this.spriteBatch.End();

            if (this.oldViewport.HasValue)
            {
                this.game.GraphicsDevice.Viewport = this.oldViewport.Value;
                this.oldViewport = null;
            }
        }
    }
}