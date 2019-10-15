using System;

namespace Snek
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleMonitor cm = new ConsoleMonitor(30, 20);
            GameManager gm = new GameManager(cm);

            gm.RunGame();
        }
    }
}
