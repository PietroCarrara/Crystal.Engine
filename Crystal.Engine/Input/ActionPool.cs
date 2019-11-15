using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Crystal.Engine.Input
{
    public class ActionPool
    {
        private Dictionary<string, CrystalInputAction> actions = new Dictionary<string, CrystalInputAction>();

        public void Add(CrystalInputAction action, string name)
        {
            this.actions.Add(name, action);
        }

        public CrystalInputAction Get(string name)
        {
            return this.actions[name];
        }
    }
}