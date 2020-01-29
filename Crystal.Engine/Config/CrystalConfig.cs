using System.IO;
using Crystal.Engine.Input;
using Crystal.Engine.Graphics;

namespace Crystal.Engine.Config
{
    public struct CrystalConfig
    {
        public ActionPool Actions;
        public string MainScene;
        public string Project;
        public ScaleStrategy ScaleStrategy;

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
                Actions = new ActionPool(),
                MainScene = "main",
                Project = "",
                ScaleStrategy = ScaleStrategy.LetterBoxing,
            };
        }
    }
}