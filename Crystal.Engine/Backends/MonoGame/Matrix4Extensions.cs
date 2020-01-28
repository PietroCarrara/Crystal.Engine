using Microsoft.Xna.Framework;
using Crystal.Framework.Math;

namespace Crystal.Engine.Backends.MonoGame
{
    public static class Matrix4Extensions
    {
        public static Matrix ToMonoGame(this Matrix4 self)
        {
            var data = self.ToFloatArray();
            
            return new Matrix(
                data[0, 0],
                data[0, 1],
                data[0, 2],
                data[0, 3],
                data[1, 0],
                data[1, 1],
                data[1, 2],
                data[1, 3],
                data[2, 0],
                data[2, 1],
                data[2, 2],
                data[2, 3],
                data[3, 0],
                data[3, 1],
                data[3, 2],
                data[3, 3]
            );
        }
    }
}