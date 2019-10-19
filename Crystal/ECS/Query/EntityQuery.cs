using System;
using System.Linq;
using System.Collections.Generic;

namespace Crystal.ECS.Query
{
    public class EntityQuery
    {
        private readonly Func<Entity, bool> selector;

        public EntityQuery()
        {
            this.selector = (e) => true;
        }

        /// <summary>
        /// Creates a query based on another query
        /// </summary>
        /// <param name="prev">The previous query selector</param>
        /// <param name="curr">The selector of this query</param>
        private EntityQuery(Func<Entity, bool> prev, Func<Entity, bool> curr)
        {
            this.selector = (e) => prev(e) && curr(e);
        }

        public IEnumerable<Entity> Run(Scene s)
        {
            return s.Entities.Where(this.selector);
        }

        public EntityQuery HasComponents(params Type[] components)
        {
            return new EntityQuery(
                this.selector,
                (e) => e.Components.ContainsAll(components)
            );
        }
    }
}