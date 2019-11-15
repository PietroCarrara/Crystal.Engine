using System;
using Crystal.Engine.Config;
using Crystal.Engine.Reflection;
using Crystal.Engine.SceneUtil.Loaders;

namespace Crystal.Engine
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            RegisteredTypes.LoadAllIn("Assemblies/");
            SceneLoader.FromDirectoryPath("Scenes/");

            var config = CrystalConfig.FromDirectory("Config/");

            using (var game = new CrystalGame(config))
                game.Run();
        }
    }
}
