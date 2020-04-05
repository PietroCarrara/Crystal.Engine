using CrystalColor = Crystal.Framework.Color;
using Microsoft.Xna.Framework;

namespace Crystal.Engine
{
    public static class ColorExtensions
    {
        public static Color ToMonoGame(this CrystalColor self)
        {
            return new Color(
                self.R,
                self.G,
                self.B,
                self.A
            );
        }

        public static CrystalColor ToCrystal(this Color self)
        {
            return new CrystalColor(
                self.R,
                self.G,
                self.B,
                self.A
            );
        }
    }
}