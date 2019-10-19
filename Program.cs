using System;

namespace Crystal.Engine
{
    public static class Program
    {   
        [STAThread]
        static void Main()
        {
            using (var game = new CrystalGame())
                game.Run();
        }
    }
}
