using System;
using TriangTriang.Controls;
using TriangTriang.Models;

namespace TriangTriang.View
{
    class Program
    {
        static Control control;

        public static int GameState { get; private set; }

        public static bool CurrentPlayer { get; private set; } = true; // piece 1 (player false) |  piece 2 (player true)
        public static int PieceChoosen { get; private set; } // ( 0 - 14 ) Piece index

        static void Main(string[] args)
        {
            GameState = 1;
            control = new Control();
            control.Initialize();

            while (GameState != 0)
            {
                int rightColor = 1;
                if (CurrentPlayer)
                    rightColor = 2;

                Map map = control.GetMap();

                switch (GameState)
                {
                    case 1: // choose what piece to move
                        ChoosePiece();
                        string input = Console.ReadLine();
                        break;
                }
            }
        }

        private static void ChoosePiece()
        {
            Map map = control.GetMap();
            Console.WriteLine(map.FindMovablePieces(CurrentPlayer));

        }

        public static void ChangeGameState(int new_game_state) => GameState = new_game_state;
    }
}
