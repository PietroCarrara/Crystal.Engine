using System;

namespace Crystal.ECS
{
    public sealed class Entity
    {
        public readonly string Name;

        internal ComponentStorage Components { get; private set; } = new ComponentStorage();

        public Entity(string name = null)
        {
            this.Name = name;
        }

        /// <summary>
        /// Adds a component to the entity
        /// </summary>
        /// <param name="c">The component to be added</param>
        public T Add<T>(T c) where T : IComponent
        {
            this.Components.Add(c);

            return c;
        }

        /// <summary>
        /// Add a component to the entity
        /// </summary>
        /// <param name="c"></param>
        /// <returns>The entity</returns>
        public Entity With<T>(T c) where T : IComponent
        {
            this.Add(c);

            return this;
        }

        public T GetComponent<T>() where T : IComponent
        {
            return this.Components.FindFirst<T>();
        }
    }
}