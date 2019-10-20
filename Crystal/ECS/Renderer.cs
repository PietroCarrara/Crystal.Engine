using Microsoft.Xna.Framework.Graphics;

namespace Crystal.ECS
{
    public interface IRenderer
    {
        void Render(Scene s, SpriteBatch sp);
    }
}