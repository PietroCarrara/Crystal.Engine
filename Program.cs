using System;
using System.Reflection;
using Crystal.Engine.SceneUtil.Loaders;

namespace Crystal.Engine
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            SceneLoader.FromFilePath("scene.yml");

            using (var game = new CrystalGame("main"))
                game.Run();
        }
    }
}
