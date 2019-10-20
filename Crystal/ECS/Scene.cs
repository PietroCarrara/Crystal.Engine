using Crystal.ECS.Collections.Specialized;

namespace Crystal.ECS
{
    public class Scene
    {
        internal EntityStorage Entities { get; private set; } = new EntityStorage();
        internal SystemStorage Systems { get; private set; } = new SystemStorage();

        /// <summary>
        /// Add a entity to this scene
        /// </summary>
        /// <param name="entity">The entity to be added</param>
        /// <returns>The added entity</returns>
        public Entity Add(Entity entity)
        {
            this.Entities.Add(entity);
            return entity;
        }

        /// <summary>
        /// Adds a system to this collection
        /// </summary>
        /// <param name="s">The system to be added</param>
        /// <returns>The added system</returns>
        public ISystem Add(ISystem s)
        {
            this.Systems.Add(s);
            return s;
        }

        /// <summary>
        /// Create a entity, add it to the scene and return it
        /// </summary>
        /// <returns>The newly created entity</returns>
        public Entity Entity(string name = null)
        {
            return this.Add(new Entity(name));
        }
    }
}