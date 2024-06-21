using System;
using TriangTriang.Controls;
using TriangTriang.Models;

namespace TriangTriang.View
{
    class Program
    {
        static Control control;
        public static bool CurrentPlayer { get; private set; } = true; // piece 1 (player false) |  piece 2 (player true)
        public static int PieceChoosen { get; private set; } // ( 0 - 14 ) Piece index

        static void Main(string[] args)
        {
            control = new Control();
            control.Initialize();

            ShowMap();
        }

        private static void ShowMap()
        {
            Map map = control.GetMap();
            Console.WriteLine(map.VisualizePossibilities(PieceChoosen));
        }
    }
}
