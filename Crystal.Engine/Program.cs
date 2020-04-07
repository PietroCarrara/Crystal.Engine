using System;
using Crystal.Engine.Config;
using Crystal.Engine.Reflection;

namespace Crystal.Engine
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var config = CrystalConfig.FromDirectory("Config/");
            RegisteredTypes.LoadAllIn("Assemblies/");

            var game = new CrystalGame(config);

            game.Run();
        }
    }
}
