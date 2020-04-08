using SFMLColor = SFML.Graphics.Color;
using CrystalColor = Crystal.Framework.Graphics.Color;

namespace Crystal.Engine.SFML
{
    public static class ColorExtensions
    {
        public static SFMLColor ToSFML(this CrystalColor self)
        {
            return new SFMLColor(
                self.R,
                self.G,
                self.B,
                self.A
            );
        }

        public static CrystalColor ToCrystal(this SFMLColor self)
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