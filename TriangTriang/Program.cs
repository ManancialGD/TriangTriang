using System;
using System.Collections.Generic;
using TriangTriang.Controls;
using TriangTriang.Models;

namespace TriangTriang.View
{
    /// <summary>
    /// This class is the View,
    /// will handle everything as showing stuff on console.
    /// </summary>
    class Program
    {
        static Control control;
        /// <summary>
        /// This is the Main game cycle.
        /// will make some space and print the map,
        /// then will ask an position of a piece to move,
        /// then the position to where the player wants to move it.
        /// </summary>
        static void Main()
        {
            control = new Control();

            while (!control.GetGameEnded())
            {
                SpaceBeforePrinting();

                // Check who's turn it is.
                if (control.GetIsPlayer1Turn()) Console.WriteLine("Player1 turn.");
                else Console.WriteLine("Player2 turn.");

                
                PrintMap(control.GetMap());

                PrintPieceTypes();

                // Input handling:
                if (SelectAPiece())
                {
                    if (WhereToMove()) continue;
                    else Console.WriteLine("Can't move to that location.");
                }
            }
        }

        /// <summary>
        /// Will ask the player a piece that he wants to move.
        /// and will read the input and tell to the controller
        /// that there was an input and it take care of checking the coords.
        /// also will cancel the game if you type "quit"
        /// </summary>
        /// <returns>True if the Input is valid</returns>
        static bool SelectAPiece()
        {
            Console.WriteLine("Choose a piece (ex: a1, b2, d3):");
            string answer = Console.ReadLine().Trim().ToLower();

            if (answer == "quit") // Check for quit
            {
                control.SetGameEnded(true);
                return false;
            }

            if (answer.Length != 2) // Check the inut Length.
            {
                Console.WriteLine($"Invalid input: {answer}");
                return SelectAPiece(); // ask again
            }
            // if the first char of the input isn't a letter and the second isn't a number, ask again. 
            if (!(char.IsLetter(answer[0]) && char.IsDigit(answer[1]))) return SelectAPiece();
            if (control.ChoseThePiece(answer)) return true; // if the input is valid (coordenates) will proceed.
            else
            {
                return SelectAPiece(); // ask again
            }
        }

        /// <summary>
        /// Will ask to the player where do he wants to move the piece to.
        /// Will tell the controller that there was an input and it will.
        /// take care of checking the coords.
        /// </summary>
        /// <returns>True if location is valid</returns>
        static bool WhereToMove()
        {
            Console.WriteLine("Where do you want to move it? (ex: a1, b2, d3):");
            string answer = Console.ReadLine().Trim().ToLower();

            if (answer.Length != 2)
            {
                Console.WriteLine($"Invalid input: {answer}");
                return false;
            }

            if (control.WantsToMoveTo(answer)) return true;
            else return false;
        }

        /// <summary>
        /// Will print the pieces types.
        /// </summary>
        static void PrintPieceTypes()
        {
            Console.WriteLine();
            Console.WriteLine("⚪ - This is Player1 pieces");
            Console.WriteLine("⚫ - This is Player2 pieces");
            Console.WriteLine("⊙ - This an empty space");
        }

        /// <summary>
        /// Ths swill take a matrix and will print it as a map in the screen.
        /// This verifies the correct i (rows) and j (columns).
        /// to know what it should do deppending on what char is in that place.
        /// </summary>
        /// <param name="map"> Receive the map that will be drawn on console. </param>
        static void PrintMap(Piece[,] map)
        {
            int rows = map.GetLength(0); // Get the amount of rows
            int cols = map.GetLength(1); // Get the amount of cols

            // Define character widths for different symbols
            Dictionary<char, int> characterWidths = new Dictionary<char, int>
            {
                { '⚪', 0 },
                { '⚫', 0 },
                { '⊗', 2 },
                { '⊙', 2 }
            };

            // This will be on top of the map, showing the columns numbers.
            Console.WriteLine("    1  2  3  4  5");

            // Rows
            for (int i = 0; i < rows; i++)
            {
                // Kinda stupid, but I didn't find another way.
                // This will add the correspond letter to the
                // begining of the row
                if (i == 0)
                {
                    Console.Write("A  ");
                }
                else if (i == 1)
                {
                    Console.Write("B  ");
                }
                else if (i == 2)
                {
                    Console.Write("C  ");
                }
                else if (i == 3)
                {
                    Console.Write("D  ");
                }
                else if (i == 4)
                {
                    Console.Write("E  ");
                }

                for (int j = 0; j < cols; j++) // Cols
                {
                    char symbol = map[i, j].ToString()[0];
                    int width = characterWidths.ContainsKey(symbol) ? characterWidths[symbol] : 2;

                    if (i == 0 || i == 4) // if the row is A or E
                    {
                        Console.Write(symbol.ToString().PadRight(width) + "    ");

                    }
                    else if (i == 1 || i == 3) // if it's B or D
                    {
                        if (j == 0) // will add a little space befor printing the row
                        {
                            Console.Write("   ");
                            width = characterWidths.ContainsKey(symbol) ? characterWidths[symbol] : 2;
                            Console.Write(symbol.ToString().PadRight(width) + " ");
                        }
                        else
                        {
                            width = characterWidths.ContainsKey(symbol) ? characterWidths[symbol] : 2;
                            Console.Write(symbol.ToString().PadRight(width) + " ");
                        }
                    }
                    else // if it's C row
                    {
                        if (j == 0) // if it's the forst col, then it will add a space
                        {
                            Console.Write("      ");
                        }
                        else if (j == 1) Console.Write(symbol.ToString().PadRight(width)); // only if it's col 3.
                    }
                }
                Console.WriteLine(); // separate by a line
            }
        }

        /// <summary>
        /// This will make some spaces before printing everything again.
        /// </summary>
        static private void SpaceBeforePrinting()
        {
            for (int i = 0; i < 15; i++)
            {
                Console.WriteLine();
            }
        }
    }
}
