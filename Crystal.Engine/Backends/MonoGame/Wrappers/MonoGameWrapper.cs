using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
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
        /// <summary>
        /// Wraps an object into a wrapper
        /// If there is no wrapper available, returns the object
        /// </summary>
        /// <param name="content">The object to wrap</param>
        /// <returns>The wrapped object or the untouched object</returns>
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
            if (content is SpriteFont sprFnt)
            {
                return new SpriteFontWrapper(sprFnt);
            }
            if (content is ContentManager cntMgr)
            {
                return new CrystalContentManager(cntMgr);
            }

            return content;
        }
    }
}