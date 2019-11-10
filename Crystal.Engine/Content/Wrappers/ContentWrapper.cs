using System;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Content.Wrappers
{
    public abstract class ContentWrapper<T>
    {
        public T Resource;

        public ContentWrapper(T content)
        {
            this.Resource = content;
        }
    }

    public abstract class ContentWrapper
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