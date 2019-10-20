using System;
using System.Collections.Generic;
using Crystal.ECS.Components;
using Crystal.ECS.Systems.Graphical;
using Crystal.ECS.Components.Graphical;
using Crystal.Engine.Scenes;

namespace Crystal.Engine
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new CrystalGame("main"))
                game.Run();
        }
    }
}
