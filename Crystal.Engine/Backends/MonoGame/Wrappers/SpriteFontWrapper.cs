using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Crystal.Framework.UI;

namespace Crystal.Engine.Backends.MonoGame.Wrappers
{
    public class SpriteFontWrapper : MonoGameWrapper<SpriteFont>, IFont
    {
        public SpriteFontWrapper(SpriteFont sp) : base(sp)
        {
        }

        public Vector2 MeasureString(string str) => this.Resource.MeasureString(str).ToCrystal();
    }
}