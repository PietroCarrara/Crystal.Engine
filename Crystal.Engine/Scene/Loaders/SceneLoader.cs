using System;
using System.IO;

namespace Crystal.Engine.Scene.Loaders
{
    public static class SceneLoader
    {
        public static SceneInitializer FromFilePath(string path)
        {
            var ext = Path.GetExtension(path);

            switch (ext)
            {
                case ".yml":
                    return new YamlSceneLoader().FromFilePath(path);
                default:
                    throw new Exception($"File type \"{ext}\" not recognized!");
            }
        }
    }
}