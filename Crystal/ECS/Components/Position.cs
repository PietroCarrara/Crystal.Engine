using Crystal.ECS;
using Microsoft.Xna.Framework;

namespace Crystal.ECS.Components
{
    public class Position : IComponent
    {
        private Vector2 position;

        public float X
        {
            get => position.X;
            set => position.X = value;
        }

        public float Y
        {
            get => position.Y;
            set => position.Y = value;
        }

        public Position(Vector2 position)
        {
            this.position = position;
        }

        public Position(float x, float y) : this(new Vector2(x, y))
        { }

        public static implicit operator Vector2(Position p)
        {
            return p.position;
        }

        public static implicit operator Position(Vector2 v)
        {
            return new Position(v);
        }
    }
}