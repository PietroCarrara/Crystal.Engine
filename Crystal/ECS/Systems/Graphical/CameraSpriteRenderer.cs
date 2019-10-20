using System.Linq;
using Crystal.ECS.Query;
using Crystal.ECS.Components;
using Crystal.ECS.Components.Graphical;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Crystal.ECS.Systems.Graphical
{
    public class CameraSpriteRenderer : IRenderer
    {
        public void Render(Scene s, SpriteBatch sp)
        {
            var camera = new EntityQuery()
                .WhereComponent<Camera>(c => c.Active)
                .Run(s)
                .FirstOrDefault();

            if (camera == null)
            {
                return;
            }

            var sprites = new EntityQuery()
                .HasComponents(typeof(Sprite), typeof(Position))
                .Run(s)
                .OrderBy((e) => e.FindFirst<Sprite>().Index);

            var pairs = sprites
            .SelectMany((e) =>
            {
                var pos = e.FindFirst<Position>();

                return e.FindAll<Sprite>()
                       .Select(sprite => (Sprite: sprite, Position: pos));
            })
            .OrderBy(e => e.Sprite.Index);

            foreach (var entity in pairs)
            {
                sp.Draw(
                    entity.Sprite.Texture,
                    entity.Position,
                    null,
                    Color.White,
                    entity.Sprite.Rotation,
                    entity.Sprite.Origin,
                    new Vector2(1),
                    SpriteEffects.None,
                    1f
                );
            }
        }
    }
}