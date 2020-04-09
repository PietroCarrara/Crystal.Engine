using System;

namespace Crystal.Engine.SDL
{
    /// <summary>
    /// Wraps SDL pointers for type-safety
    /// </summary>
    public class SDLSafeType
    {
        protected IntPtr pointer;

        public SDLSafeType(IntPtr p)
        {
            pointer = p;
        }

        public static implicit operator IntPtr(SDLSafeType t)
        {
            return t.pointer;
        }
    }
}