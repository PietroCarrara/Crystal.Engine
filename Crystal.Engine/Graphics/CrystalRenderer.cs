using System;
using System.Numerics;
using Crystal.Framework;
using Crystal.Framework.Graphics;
using Crystal.Framework.UI;
using Crystal.Engine.SFML;
using SFML.Graphics;
using Color = Crystal.Framework.Graphics.Color;

namespace Crystal.Engine.Graphics
{
    public class CrystalRenderer : IDrawer
    {
        private RenderTexture target;
        private Transform transform;

        public void BeginDraw(IRenderTarget target,
                              TextureSlice? viewport = null, // TODO
                              Matrix4x4? transformMatrix = null,
                              SamplerState samplerState = SamplerState.LinearClamp)
        {
            this.target = target.ToSFML();
            this.transform = transformMatrix.HasValue ?
                transformMatrix.Value.ToSFML() :
                Transform.Identity;
        }

        public void Draw(IDrawable texture,
                         Vector2 position,
                         Color color,
                         float deltaTime,
                         Vector2? origin = null,
                         float rotation = 0,
                         Vector2? scale = null,
                         TextureSlice? sourceRectangle = null)
        {
            var originValue = origin.Value;
            if (!origin.HasValue)
            {
                originValue = new Vector2(.5f);
            }

            var scaleValue = scale.Value;
            if (!scale.HasValue)
            {
                scaleValue = new Vector2(1);
            }

            var originPoint = texture.Size * originValue;

            var spr = new Sprite(texture.ToSFML());
            spr.Position = position.ToSFML();
            spr.Color = color.ToSFML();
            spr.Origin = originPoint.ToSFML();
            spr.Rotation = (MathF.PI / 180) * rotation;
            spr.Scale = scaleValue.ToSFML();
            if (sourceRectangle.HasValue)
            {
                spr.TextureRect = sourceRectangle.Value.ToSFML();
            }

            target.Draw(spr, new RenderStates(transform));
        }

        public void Draw(IDrawable texture,
                         TextureSlice destinationRectangle,
                         Color color,
                         float deltaTime,
                         Vector2? origin = null,
                         float rotation = 0,
                         TextureSlice? sourceRectangle = null)
        {
            // TODO
        }

        public void DrawString(IFont font,
                               Vector2 position,
                               Color color,
                               string text,
                               Vector2? scale = null,
                               TextureSlice? sourceRectangle = null,
                               float rotation = 0)
        {
            // TODO
        }

        public void EndDraw()
        {
            this.target.Display();
            this.target = null;
        }
    }
}