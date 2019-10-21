using System;
using System.IO;
using System.Linq;

namespace Crystal.Engine.SceneUtil.Loaders
{
    public class SceneLoader : ISceneLoader
    {
        public SceneInitializer FromFilePath(string path)
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

        public SceneInitializer FromText(string text)
        {
            throw new Exception("Can't automatically determine the file format only through text!");
        }
    }
}