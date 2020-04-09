using System.Numerics;
using SFML.Graphics;

namespace Crystal.Engine.SFML
{
    public static class Matrix4x4Extensions
    {
        public static Transform ToSFML(this Matrix4x4 self)
        {
            // FIXME: SFML only allows for 3x3 transforms,
            // since it doesn't support 3D
            return new Transform(
                self.M11, self.M21, self.M41,
                self.M12, self.M22, self.M42,
                self.M13, self.M23, self.M43
            );
        }
    }
}