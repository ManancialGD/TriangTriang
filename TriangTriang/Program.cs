using System;
using TriangTriang.Controls;
using TriangTriang.Models;

namespace TriangTriang.View
{
    class Program
    {
        static Control control;

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

        }
    }
}
