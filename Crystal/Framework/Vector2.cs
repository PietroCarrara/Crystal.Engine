namespace Crystal.Framework
{
    public struct Vector2
    {
        public static Vector2 Zero { get; } = new Vector2(0, 0);
        public static Vector2 One { get; } = new Vector2(1, 1);

        public float X, Y;

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2(float xy) : this(xy, xy)
        { }
    }
}