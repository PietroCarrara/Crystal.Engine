namespace Crystal.ECS
{
    public class Scene
    {
        internal EntityStorage Entities { get; private set; } = new EntityStorage();

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
        /// Create a entity, add it to the scene and return it
        /// </summary>
        /// <returns>The newly returned entity</returns>
        public Entity Entity(string name = null)
        {
            return this.Add(new Entity(name));
        }
    }
}