using System;
using System.IO;
using System.Collections.Generic;

namespace Crystal.Engine.SceneUtil.Loaders
{
    public static class SceneLoader
    {
        public static SceneInitializer FromFilePath(string path)
        {
            try
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
            catch (TypeLoadException e)
            {
                throw new Exception($"Type \"{e.TypeName}\" wasn't found. Maybe you are " +
                                    "missing an assembly reference?");
            }
        }

        public static List<SceneInitializer> FromDirectoryPath(string path)
        {
            var res = new List<SceneInitializer>();

            foreach (var file in Directory.GetFiles(path))
            {
                res.Add(SceneLoader.FromFilePath(file));    
            }

            return res;
        }
    }
}