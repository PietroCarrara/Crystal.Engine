using System;
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

            throw new Exception($"Content of type {content.GetType()} can't be wrapped!");
        }
    }
}