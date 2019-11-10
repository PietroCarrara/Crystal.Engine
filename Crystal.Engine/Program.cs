using System;
using System.Reflection;
using Crystal.Engine.SceneUtil.Loaders;
using Crystal.Engine.Reflection;

namespace Crystal.Engine
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            RegisteredTypes.LoadAllIn("Assemblies/");
            SceneLoader.FromDirectoryPath("Scenes/");

            using (var game = new CrystalGame("main"))
                game.Run();
        }
    }
}
