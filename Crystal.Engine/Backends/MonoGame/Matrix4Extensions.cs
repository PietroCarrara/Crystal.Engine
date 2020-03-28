using System.Numerics;
using Microsoft.Xna.Framework;

namespace Crystal.Engine
{
    public static class Matrix4Extensions
    {
        public static Matrix ToMonoGame(this Matrix4x4 self)
        {
            return new Matrix(
                self.M11,
                self.M12,
                self.M13,
                self.M14,
                self.M21,
                self.M22,
                self.M23,
                self.M24,
                self.M31,
                self.M32,
                self.M33,
                self.M34,
                self.M41,
                self.M42,
                self.M43,
                self.M44
            );
        }
    }
}