using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Backends.MonoGame.Wrappers
{
    public abstract class MonoGameWrapper<T>
    {
        public T Resource { get; }

        public MonoGameWrapper(T content)
        {
            this.Resource = content;
        }
    }

    public sealed class MonoGameWrapper
    {
        public static object Wrap(object content)
        {
            if (content is Texture2D t)
            {
                return new Texture2DWrapper(t);
            }
            if (content is SoundEffect se)
            {
                return new SoundEffectWrapper(se);
            }
            if (content is Song s)
            {
                return new SongWrapper(s);
            }
            if (content is RenderTarget2D rt2d)
            {
                return new RenderTarget2DWrapper(rt2d);
            }

            throw new Exception($"Content of type {content.GetType()} can't be wrapped!");
        }
    }
}