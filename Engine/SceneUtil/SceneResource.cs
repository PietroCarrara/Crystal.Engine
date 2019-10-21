namespace Crystal.Engine.SceneUtil
{
    /// <summary>
    /// Denotes a reference to a scene resource
    /// Used when initializing scenes to refer to a resource
    /// that may not yet be loaded
    /// </summary>
    public class SceneResource
    {
        public readonly string Name;

        public SceneResource(string name)
        {
            this.Name = name;
        }
    }
}