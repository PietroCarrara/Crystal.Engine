using System.Numerics;
using SFML.System;

namespace Crystal.Engine.SFML
{
    public static class Vector2Extensions
    {
        public static Vector2f ToSFML(this Vector2 self)
        {
            return new Vector2f(self.X, self.Y);
        }

        public static Vector2 ToCrystal(this Vector2f self)
        {
            return new Vector2(self.X, self.Y);
        }

        public static Vector2 ToCrystal(this Vector2i self)
        {
            return new Vector2(self.X, self.Y);
        }
    }
}