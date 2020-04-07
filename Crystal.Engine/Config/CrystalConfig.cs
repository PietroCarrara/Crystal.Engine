using System.IO;

namespace Crystal.Engine.Config
{
    public struct CrystalConfig
    {
        public string MainScene;
        public string Project;

        public static CrystalConfig FromDirectory(string dir)
        {
            var fileName = Path.Combine(dir, "config.yml");
            if (File.Exists(fileName))
            {
                return ConfigParser.FromFile(fileName);
            }

            // Return default values
            return Default;
        }

        public static CrystalConfig Default
        {
            get => new CrystalConfig
            {
                MainScene = "main",
                Project = "",
            };
        }
    }
}