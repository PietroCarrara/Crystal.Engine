using Crystal.Framework.Graphics;
using SFML.Graphics;

namespace Crystal.Engine.SFML
{
    public static class TextureSliceExtensions
    {
        public static IntRect ToSFML(this TextureSlice self)
        {
            return new IntRect(
                self.TopLeft.X,
                self.TopLeft.Y,
                self.Width,
                self.Height
            );
        }
    }
}