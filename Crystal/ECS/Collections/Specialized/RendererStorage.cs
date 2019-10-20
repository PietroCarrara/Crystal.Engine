using System.Collections;
using System.Collections.Generic;

namespace Crystal.ECS.Collections.Specialized
{
    internal class RendererStorage : IEnumerable<IRenderer>
    {
        private List<IRenderer> data = new List<IRenderer>();
        
        public IEnumerator<IRenderer> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public void Add(IRenderer s)
        {
            this.data.Add(s);
        }
    }
}