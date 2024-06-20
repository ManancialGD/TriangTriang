using System;
using TriangTriang.Controls;
using TriangTriang.Models;

namespace TriangTriang.View
{
    class Program
    {
        static Control control;
        public static bool CurrentPlayer {get; private set;} = true; // piece 1 (player false) |  piece 2 (player true)

        static void Main(string[] args)
        {
            control = new Control();
            control.Initialize();

            ShowMap();
        }

        private static void ShowMap()
        {
            Map map = control.GetMap();
            Console.WriteLine(map.Visualize());
            map.FindPossiblePlays(CurrentPlayer, 10);

        }
    }
}
