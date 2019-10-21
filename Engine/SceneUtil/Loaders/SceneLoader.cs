using System;
using System.IO;

namespace Crystal.Engine.SceneUtil.Loaders
{
    public class SceneLoader : ISceneLoader
    {
        public SceneInitializer FromFilePath(string path)
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

        public SceneInitializer FromText(string text)
        {
            throw new Exception("Can't automatically determine the file format only through text!");
        }
    }
}