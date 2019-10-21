namespace Crystal.Engine.SceneUtil.Loaders
{
    public interface ISceneLoader
    {
        SceneInitializer FromText(string text);
        SceneInitializer FromFilePath(string path);
    }
}