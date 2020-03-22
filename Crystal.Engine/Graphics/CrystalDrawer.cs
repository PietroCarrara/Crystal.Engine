using System.Numerics;
using Crystal.Framework.UI;
using Crystal.Framework.Graphics;
using Crystal.Engine.Backends.MonoGame;
using Microsoft.Xna.Framework.Graphics;
using CrystalSampler = Crystal.Framework.Graphics.SamplerState;
using MGVec2 = Microsoft.Xna.Framework.Vector2;
using Matrix = Microsoft.Xna.Framework.Matrix;

namespace Crystal.Engine.Graphics
{
    public class CrystalDrawer : IDrawer
    {
        private SpriteBatch spriteBatch;

        private Viewport oldViewport;

        public CrystalDrawer(CrystalGame game)
        {
            this.spriteBatch = game.SpriteBatch;
        }

        public void BeginDraw(
            IRenderTarget target,
            TextureSlice? viewport = null,
            Matrix4x4? transformMatrix = null,
            CrystalSampler samplerState = CrystalSampler.LinearClamp)
        {
            oldViewport = spriteBatch.GraphicsDevice.Viewport;
            
            spriteBatch.GraphicsDevice.SetRenderTarget(target.ToMonoGame());

            if (viewport.HasValue)
            {
                spriteBatch.GraphicsDevice.Viewport = new Viewport(viewport.Value.ToMonoGame());
            }

            Matrix? matrix = null;
            if (transformMatrix.HasValue)
            {
                matrix = transformMatrix.Value.ToMonoGame();
            }

            spriteBatch.Begin(
                transformMatrix: matrix,
                samplerState: samplerState.ToMonogame()
            );
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
            orig *= sourceRectangle.HasValue
                                    ? new Vector2(sourceRectangle.Value.Width, sourceRectangle.Value.Height)
                                    : new Vector2(texture.Width, texture.Height);

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

        public void Draw(IDrawable texture,
                         TextureSlice destinationRectangle,
                         float deltaTime,
                         Vector2? origin = null,
                         float rotation = 0,
                         TextureSlice? sourceRectangle = null)
        {
            var tex = texture.ToTexture2D();

            var orig = origin.HasValue
                                ? origin.Value
                                : new Vector2(0.5f, 0.5f);
            orig *= sourceRectangle.HasValue
                                    ? new Vector2(sourceRectangle.Value.Width, sourceRectangle.Value.Height)
                                    : new Vector2(texture.Width, texture.Height);

            this.spriteBatch.Draw(
                tex,
                destinationRectangle.ToMonoGame(),
                sourceRectangle?.ToMonoGame(),
                Microsoft.Xna.Framework.Color.White,
                rotation,
                orig.ToMonoGame(),
                SpriteEffects.None,
                0
            );
        }

        public void DrawString(IFont font, Vector2 position, string text, Vector2? scale = null, float rotation = 0)
        {
            var fon = font.ToMonoGame();
            var pos = position.ToMonoGame();
            var scl = scale.HasValue ? scale.Value.ToMonoGame() : MGVec2.One;

            this.spriteBatch.DrawString(
                fon,
                text,
                pos,
                Microsoft.Xna.Framework.Color.Black,
                rotation,
                MGVec2.One,
                scl,
                SpriteEffects.None,
                0
            );
        }

        public void EndDraw()
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = oldViewport;
        }
    }
}