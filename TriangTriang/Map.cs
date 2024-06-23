using System;
using System.Security.AccessControl;

namespace TriangTriang.Models
{
    public class MapManager
    {
        private Piece[,] map;
        private int[] selectedPiece;
        private string currentPlayerPiece = "⚪";
        private string currentEnemyPiece = "⚫";

        /// <summary>
        /// Will create the map and
        /// create an empty array[2] for the
        /// SelectedPiece.
        /// </summary>&
        public MapManager()
        {
            map = new Piece[5, 3]
            {
                { new Piece('⚪'),      new Piece('⚪'),     new Piece('⚪') },
                {      new Piece('⚪'), new Piece('⚪'),  new Piece('⚪') },
                {      new Piece('⊗'),  new Piece('⊙'), new Piece('⊗') },
                {       new Piece('⚫'), new Piece('⚫'), new Piece('⚫') },
                { new Piece('⚫'),       new Piece('⚫'),      new Piece('⚫') }
            };
            selectedPiece = new int[2];
        }

        /// <summary>
        /// This will check if the selected piece is valid.
        /// By seeing if the piece is the current player's piece
        /// and will check if that piece can move.
        /// </summary>
        /// <param name="coords">The selected piece to move coords as [y,x]</param>
        /// <returns>true if it's a valid piece and can move</returns>
        public bool CheckIfSelectedAValidPiece(int[] coords)
        {
            // First of all, this will check if the selected piece is the currene player's
            if (map[coords[0], coords[1]].ToString() == currentPlayerPiece)
            {
                selectedPiece = coords;
                bool hasAMove = false; // Flag to wether the piece an move or not


                // This will try to move to a coord.
                // Rows
                for (int i = -4; i < 4; i++)
                {
                    // Cols
                    for (int j = -4; j < 4; j++)
                    {

                        // target try by each row and col.
                        int[] targetTry = new int[2];
                        targetTry[0] = coords[0] + j;
                        targetTry[1] = coords[1] + i;

                        // Check the distance.
                        int distY = (int)MathF.Abs(coords[0] - targetTry[0]);
                        int distX = (int)MathF.Abs(coords[1] - targetTry[1]);

                        if (distX > 2 || distY > 2 || targetTry[0] < 0 || targetTry[1] < 0) // prevent from going in a distace > 2 or to a "-" pos.
                        {
                            hasAMove = false;
                        }
                        else if (IsJumpMove(targetTry[1], targetTry[0], distX, distY, out int[] midPos))
                        {
                            hasAMove = true;
                            goto endOfLoop; // once found, stop the loop
                        }
                        else if (IsAdjacentMove(distX, distY, targetTry))
                        {
                            hasAMove = true;
                            goto endOfLoop; // once found, stop the loop
                        }
                        else
                        {
                            hasAMove = false;
                        }
                    }
                }

            endOfLoop:

                if (hasAMove)
                {
                    selectedPiece = coords;
                    return true;
                }
                else
                {
                    Console.WriteLine("That piece can't move");
                    return false;
                }
            }
            Console.WriteLine("That piece isn't yours"); // selected an enemy's piece
            return false;
        }

        /// <summary>
        /// This will check if the pos the player
        /// want to move the pice is valid, and move it.
        /// </summary>
        /// <param name="targetCoords">Place where the player wants to move the piece to</param>
        /// <returns>True if the position is valid.</returns>
        public bool CheckIfSelectedAValidTarget(int[] targetCoords)
        {
            int targetX = targetCoords[1];
            int targetY = targetCoords[0];

            int distX = Math.Abs(selectedPiece[1] - targetX);
            int distY = Math.Abs(selectedPiece[0] - targetY);

            if (IsAdjacentMove(distX, distY, targetCoords))
            {
                MoveToLocation(selectedPiece, new int[] { targetY, targetX });
                return true;
            }
            else if (IsJumpMove(targetX, targetY, distX, distY, out int[] midPos))
            {
                map[midPos[0], midPos[1]] = new Piece('⊙');
                MoveToLocation(selectedPiece, new int[] { targetY, targetX });
                return true;
            }
            return false;
        }

        /// <summary>
        /// This will check for side's movement.
        /// </summary>
        /// <param name="distX"> distance in X between the selected piece pos and the target pos </param>
        /// <param name="distY"> distance in Y between the selected piece pos and the target pos </param>
        /// <param name="targetCoords"> Target position. </param>
        /// <returns>True if can move adjacently</returns>
        private bool IsAdjacentMove(int distX, int distY, int[] targetCoords)
        {
            if (selectedPiece[0] % 2 == 0) // if the selected piece is in A, C or E
            {
                if ((distX == 0 && distY == 1) || (distX == 1 && distY == 0))
                {
                    return IsWithinBounds(targetCoords[0], targetCoords[1]) && map[targetCoords[0], targetCoords[1]].ToString() == "⊙";
                }
                else return false;
            }
            else // if the row is B or D
            {
                if ((distX == 0 && distY == 1) || (distX == 1 && distY == 0))
                {
                    return IsWithinBounds(targetCoords[0], targetCoords[1]) && map[targetCoords[0], targetCoords[1]].ToString() == "⊙";
                }
                else if (distX == 1 && distY == 1)
                {
                    if (targetCoords[0] == 2) return IsWithinBounds(targetCoords[0], targetCoords[1]) && map[targetCoords[0], targetCoords[1]].ToString() == "⊙";
                    else return false;
                }
                else return false;
            }
        }

        /// <summary>
        /// Determines if the move is a valid jump move over an enemy piece.
        /// </summary>
        /// <param name="targetX">Target position in X axis.</param>
        /// <param name="targetY">Target position in Y axis.</param>
        /// <param name="distX">Distance between selected piece and target position in X axis.</param>
        /// <param name="distY">Distance between selected piece and target position in Y axis.</param>
        /// <param name="midPos">Output parameter returning the mid position between selected piece and target position.</param>
        /// <returns>True if the move is a valid jump move, otherwise false.</returns>
        private bool IsJumpMove(int targetX, int targetY, int distX, int distY, out int[] midPos)
        {
            midPos = new int[2];

            // Horizontal jump
            if (distX == 2 && distY == 0)
            {
                int midX = (int)MathF.Abs((selectedPiece[1] + targetX) / 2);
                if (IsWithinBounds(selectedPiece[0], midX) && map[selectedPiece[0], midX].ToString() == currentEnemyPiece)
                {
                    if (IsWithinBounds(targetY, targetX) && map[targetY, targetX].ToString() == "⊙")
                    {
                        midPos[0] = selectedPiece[0];
                        midPos[1] = midX;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            // Diagonal jump
            else if (distX == 2 && distY == 2)
            {
                int midX = (int)MathF.Abs((selectedPiece[1] + targetX) / 2);
                int midY = (int)MathF.Abs((selectedPiece[0] + targetY) / 2);
                if (IsWithinBounds(midY, midX) && map[midY, midX].ToString() == currentEnemyPiece)
                {
                    if (IsWithinBounds(targetY, targetX) && map[targetY, targetX].ToString() == "⊙")
                    {
                        midPos[0] = midY;
                        midPos[1] = midX;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            // Vertical jump
            else if (distX == 0 && distY == 2)
            {
                int midY = (int)MathF.Abs((selectedPiece[0] + targetY) / 2);
                if (IsWithinBounds(midY, 0) && map[midY, targetX].ToString() == currentEnemyPiece)
                {
                    if (IsWithinBounds(targetY, targetX) && map[targetY, targetX].ToString() == "⊙")
                    {
                        midPos[0] = midY;
                        midPos[1] = targetX;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            // One axis distance 1, another axis distance 2
            else if (distX == 1 && distY == 2)
            {
                int midX = 0;
                if (selectedPiece[0] < 2) midX = selectedPiece[1];
                else midX = targetX;
                int midY = (int)MathF.Abs((selectedPiece[0] + targetY) / 2);
                if (IsWithinBounds(midY, midX) && map[midY, midX].ToString() == currentEnemyPiece)
                {
                    if (IsWithinBounds(targetY, targetX) && map[targetY, targetX].ToString() == "⊙")
                    {
                        midPos[0] = midY;
                        midPos[1] = midX;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            // Invalid jump move
            else
            {
                return false;
            }
        }

        // Check if the coords are within bounds of the map
        private bool IsWithinBounds(int y, int x)
        {
            return y >= 0 && y < map.GetLength(0) && x >= 0 && x < map.GetLength(1);
        }

        /// <summary>
        /// Will put an empty space where the piece are
        /// and will put the piece where the targetPos is.
        /// </summary>
        /// <param name="currentPos">The current piece location</param>
        /// <param name="targetPos">Where the piece want to g</param>
        private void MoveToLocation(int[] currentPos, int[] targetPos)
        {
            map[currentPos[0], currentPos[1]] = new Piece('⊙');
            map[targetPos[0], targetPos[1]] = new Piece(currentPlayerPiece[0]);
        }

        /// <summary>
        /// This will run through the map
        /// and will check if the piece is there
        /// </summary>
        /// <param name="piece">The piece you want to check if has in the map</param>
        /// <returns>True if there is NO pieces of that type.</returns>
        public bool CheckPiecesOfType(string piece)
        {
            int pieceCount = 0;
            foreach (Piece p in map)
            {
                if (p.ToString() == piece) pieceCount++;
            }
            if (pieceCount > 0) return false;
            else return true;
        }

        // Getters and Setters.
        public Piece[,] GetMap()
        {
            return map;
        }
        /// <summary>
        /// This will set what is the current player's pice
        /// and enemy's
        /// </summary>
        /// <param name="isPlayer1Turn">if the current player is the player1</param>
        public void SetCurrentPlayerPiece(bool isPlayer1Turn)
        {
            if (isPlayer1Turn)
            {
                currentPlayerPiece = "⚪";
                currentEnemyPiece = "⚫";
            }
            else
            {
                currentPlayerPiece = "⚫";
                currentEnemyPiece = "⚪";
            }

        }
    }
}
