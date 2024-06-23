using System;
using System.Dynamic;
using System.Runtime.InteropServices;
using TriangTriang.Models;

namespace TriangTriang.Controls
{
    /// <summary>
    /// This is the controller of our project.
    /// He will receive the inputs from the View (program)
    /// and will translate it to coordenates, will also manage if the game
    /// is over and who turn it is.
    /// </summary>
    public class Control
    {
        bool gameEnded = false;
        private MapManager mapManager;
        private bool isPlayer1Turn = true;
        
        public Control()
        {
            mapManager = new MapManager(); // Create a new map when instantiated.
        }
        
        /// <summary>
        /// If it can be translated to coords,
        /// then it will check if that pice can't move.
        /// </summary>
        /// <param name="pieceChosen">This is the input from the Player handled in program</param>
        /// <returns>True if the position is valid.</returns>
        public bool ChoseThePiece(string pieceChosen)
        {
            // Try to translate the coordenates.
            if (TranslateCoordinates(pieceChosen, out int[] coord)) // this out coord will be the coordenate already translated.
            {
                return mapManager.CheckIfSelectedAValidPiece(coord);
            }
            else
            {
                Console.WriteLine("Outside bounds piece.");
                return false;
            }
        }

        /// <summary>
        /// Check if the place the player wants to move
        /// the piece to is valid.
        /// will also change from player1 to player2
        /// and check if there is still pieces in the game
        /// </summary>
        /// <param name="place">This is the coords to where the player wants the piece to go</param>
        /// <returns>True if it's a valid coord</returns>
        public bool WantsToMoveTo(string place)
        {
            if (TranslateCoordinates(place, out int[] coord))
            {
                if (mapManager.CheckIfSelectedAValidTarget(coord))
                {
                    if (isPlayer1Turn) isPlayer1Turn = false;
                    else if (!isPlayer1Turn) isPlayer1Turn = true;

                    if (mapManager.CheckPiecesOfType("⚪"))
                    {
                        gameEnded = true;
                        Console.WriteLine("Player 2 Won.");
                        return true;
                    }
                    else if (mapManager.CheckPiecesOfType("⚫"))
                    {
                        gameEnded = true;
                        Console.WriteLine("Player 1 Won.");
                        return true;
                    }
                    else
                    {
                        mapManager.SetCurrentPlayerPiece(isPlayer1Turn);
                        return true;
                    }

                }
                return false;
            }
            else
            {
                Console.WriteLine("Outside bounds piece."); // If can't translate, the coords are out of bounds.
                return false;
            }
        }

        /// <summary>
        /// This will take the coords as "a1", "b2", "c3", "d4"
        /// And will translate to "x,y"
        /// First will check the Y, that is the letters.
        /// then will check the X, that is the number.
        /// </summary>
        /// <param name="pos">This is the coords to translate</param>
        /// <param name="coord">This is the already translated coords as [y,x]</param>
        /// <returns>True if can translate it</returns>
        private bool TranslateCoordinates(string pos, out int[] coord)
        {
            // Start varriables
            int x = 0;
            int y = 0;
            coord = new int[2];

            // Check of Y
            switch (pos[0])
            {
                case 'a':
                    y = 0;
                    break;
                case 'b':
                    y = 1;
                    break;
                case 'c':
                    y = 2;
                    break;
                case 'd':
                    y = 3;
                    break;
                case 'e':
                    y = 4;
                    break;
                default:
                    return false;
            }

            // Take the X input.
            x = int.Parse(pos.Substring(1));

            // Check for every cenarrio to translate it correctly.
            if (y == 2)
            {
                if (x != 3) return false;
            }
            if (y % 2 == 0) // if the row is A or E
            {
                switch (x)
                {
                    case 1:
                        x = 0;
                        break;
                    case 2:
                        return false;
                    case 3:
                        x = 1;
                        break;
                    case 4:
                        return false;
                    case 5:
                        x = 2;
                        break;
                    default:
                        return false;
                }
            }
            else // If the row is B or C
            {
                switch (x)
                {
                    case 1:
                        return false;
                    case 2:
                        x = 0;
                        break;
                    case 3:
                        x = 1;
                        break;
                    case 4:
                        x = 2;
                        break;
                    case 5:
                        return false;
                    default:
                        return false;

                }
            }
            coord[0] = y;
            coord[1] = x;
            return true;
        }

        // Getters and Setters.
        public Piece[,] GetMap()
        {
            return mapManager.GetMap();
        }
        public bool GetGameEnded()
        {
            return gameEnded;
        }
        public void SetGameEnded(bool b)
        {
            gameEnded = b;
        }
        public bool GetIsPlayer1Turn()
        {
            return isPlayer1Turn;
        }
    }
}
