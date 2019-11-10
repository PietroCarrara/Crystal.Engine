using Crystal.Framework.Graphics;
using Crystal.Engine.Backends.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vec2 = Crystal.Framework.Vector2;

namespace Crystal.Engine.Content.Wrappers
{
    public class Texture2DWrapper : ContentWrapper<Texture2D>, ITexture
    {
        public int Width => this.Resource.Width;

        public int Height => this.Resource.Height;

        public Texture2DWrapper(Texture2D tex) : base(tex)
        { }

        public void Draw(
            Vec2 position,
            Vec2? origin = null,
            float rotation = 0,
            Vec2? scale = null,
            TextureSlice? sourceRectangle = null,
            float layerDepth = 1
        )
        {
            var sp = BaseGame.SpriteBatch;

            // Calculating origin point
            var orig = origin.HasValue ? origin.Value : new Vec2(.5f);
            orig *= new Vec2(this.Width, this.Height);

            // Resolving scale
            var scal = scale.HasValue ? scale.Value : new Vec2(1);

            sp.Draw(
                this.Resource,
                position.ToMonoGame(),
                sourceRectangle?.ToMonoGame(),
                Microsoft.Xna.Framework.Color.White,
                rotation,
                orig.ToMonoGame(),
                scal.ToMonoGame(),
                SpriteEffects.None,
                layerDepth
            );
        }

        public void Dispose()
        {
            this.Resource.Dispose();
        }
    }
}