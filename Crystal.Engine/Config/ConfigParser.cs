using System;
using System.IO;
using Crystal.Engine.Config.Parsers;

namespace Crystal.Engine.Config
{
    public static class ConfigParser
    {
        public static CrystalConfig FromFile(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".yml":
                    return new YamlConfigParser().FromFile(path);
                default:
                    throw new Exception("Configuration file type unknown!");
            }
        }
    }
}