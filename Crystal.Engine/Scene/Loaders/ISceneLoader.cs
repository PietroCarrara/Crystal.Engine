namespace Crystal.Engine.Scene.Loaders
{
    public interface ISceneLoader
    {
        SceneInitializer FromText(string text);
        SceneInitializer FromFilePath(string path);
    }
}