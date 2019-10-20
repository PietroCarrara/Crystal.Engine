using Crystal.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.ECS.Components.Graphical
{
    public class Sprite : IComponent
    {
        /// <summary>
        /// The texture of this sprite
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Point indicating the origin of the sprite
        /// X and Y must be in range [0, 1]
        /// (0, 0) means top left
        /// (1, 1) means bottom right
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// The order in which to draw this sprite
        /// The less it is, the further back it is
        /// So a background could be on Index -1, and the rest of the game on a bigger Index
        /// </summary>
        public int Index = 0;

        /// <summary>
        /// Sprite rotarion in radians
        /// Flows on a clock-wise direction
        /// </summary>
        public float Rotation;

        public Sprite(Texture2D texture, Vector2? origin = null)
        {
            this.Texture = texture;

            if (origin.HasValue)
            {
                this.Origin = origin.Value;
            }
            else
            {
                this.Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            }
        }
    }
}