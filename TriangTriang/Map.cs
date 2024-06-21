using System;
using System.Collections.Generic;

namespace TriangTriang.Models
{
    public class Map
    {
        public int[,] Slots { get; private set; }

        /*
        [ 1, 1, 1
          1, 1, 1
         -1, 0,-1
          1, 1, 1
          1, 1, 1 ]
        */

        public Map()
        {
            Slots = new int[5, 3]
            {
                { 1, 1, 1 },
                { 2, 1, 0 },
                { -1, 1, -1 },
                { 2, 2, 0 },
                { 2, 2, 2 }
            };
        }

        /*
        〇
        ⦿⊚

        ⒶⒷⒸⒹⒺⒻ

        ⊚  ⊚  ⊚
          ⊚⊚⊚

          ⦿⦿⦿
        ⦿  ⦿  ⦿
        */

        public string Visualize()
        {
            string map = "";
            int index = 0;

            int[] firstSpace = { 1, 2, 3, 8, 9, 13, 14 };
            int[] secondSpace = { 5, 6, 11 };

            foreach (var slot in Slots)
            {
                foreach (int i in firstSpace)
                {
                    if (i == index)
                        map += " ";
                }

                switch (slot)
                {
                    case 1:
                        map += "⚪";
                        break;
                    case 2:
                        map += "⚫";
                        break;
                    default:
                        map += "  ";
                        break;
                }

                foreach (int i in secondSpace)
                {
                    if (i == index)
                        map += " ";
                }

                if ((index + 1) % 3 == 0)
                    map += "\n";

                index++;
            }
            return map;
        }

        public string Visualize_ChoosePiece(bool currentPlayer)
        {
            string map = "";
            int index = 0;
            int[] firstSpace = { 1, 2, 3, 8, 9, 13, 14 };
            int[] secondSpace = { 5, 6, 11 };

            int[] piecesPossible = new int[0];

            int[][] possiblePlays = new int[0][]; // possibilities for each piece
            int b = 0;
            foreach (int slot in Slots)
            {
                possiblePlays[b] = FindPossiblePlays(b);
                b++;
            }
            b = 0;
            foreach (int[] p in possiblePlays)
            {
                if (p.Length > 0)
                    AddToArray(ref piecesPossible, b); // piece can move
                b++;
            }

            Console.WriteLine(possiblePlays.Length);

            foreach (var slot in Slots)
            {
                foreach (int i in firstSpace)
                {
                    if (i == index)
                        map += " ";
                }

                switch (slot)
                {
                    case 1:
                        map += "⚪";
                        break;
                    case 2:
                        map += "⚫";
                        break;
                    default:
                        map += "  ";
                        break;
                }

                foreach (int i in secondSpace)
                {
                    if (i == index)
                        map += " ";
                }

                if ((index + 1) % 3 == 0)
                    map += "\n";

                index++;
            }
            return map;
        }

        public string VisualizePossibilities(int currentSlot)
        {
            string map = "";
            int index = 0;

            int[] firstSpace = { 1, 2, 3, 8, 9, 13, 14 };
            int[] secondSpace = { 5, 6, 11 };

            int[] possiblePlays = FindPossiblePlays(currentSlot);
            Console.WriteLine(possiblePlays.Length);

            foreach (var slot in Slots)
            {
                foreach (int i in firstSpace)
                {
                    if (i == index)
                        map += " ";
                }

                switch (slot)
                {
                    case 1:
                        map += "⚪";
                        break;
                    case 2:
                        map += "⚫";
                        break;
                    default:
                        map += "  ";
                        break;
                }

                foreach (int i in secondSpace)
                {
                    if (i == index)
                        map += " ";
                }

                if ((index + 1) % 3 == 0)
                    map += "\n";

                index++;
            }
            return map;
        }

        public int[] FindPossiblePlays(int currentSlot)
        { // ( 0 - 14 )
            int[] possiblePlays = new int[0];
            (int[] piecesDetected, int[] slotsIndexes) = GetCloseSlots(currentSlot);
            Console.WriteLine(GetCloseSlots(currentSlot));
            int index = 0;
            foreach (int piece in piecesDetected)
            {
                if (piece == 0)
                {
                    AddToArray(ref possiblePlays, slotsIndexes[index]);
                }
                else if (
                    piece
                    != Slots[
                        ConvertIndexToSlot(currentSlot).Item1,
                        ConvertIndexToSlot(currentSlot).Item2
                    ]
                )
                {
                    (int, int) slot = ConvertIndexToSlot(currentSlot); // index -> slot
                    (int, int) mov = (
                        ConvertIndexToSlot(slotsIndexes[index]).Item1 - slot.Item1,
                        ConvertIndexToSlot(slotsIndexes[index]).Item2 - slot.Item2
                    );

                    if (
                        GetSlot(ConvertSlotToIndex(slot.Item1 + mov.Item1, slot.Item2 + mov.Item2))
                        == 0
                    )
                    {
                        AddToArray(ref possiblePlays, slotsIndexes[index]);
                    }
                }

                index++;
            }
            return possiblePlays;
        }

        private void AddToArray(ref int[] array, int value)
        {
            int[] tmp = array;
            array = new int[array.Length + 1];

            int i = 0;
            foreach (int v in tmp)
            {
                array[i] = v;
                i++;
            }
            array[array.Length - 1] = value;
            return;
        }

        private (int[], int[]) GetCloseSlots(int slot)
        { // 0 - 14 ( index, piece? )
            Console.WriteLine("GetCloseSlots( " + slot + " )");
            switch (slot)
            {
                case 0:
                    return (
                        new int[] { Slots[0, 1], Slots[1, 0] },
                        new int[] { ConvertSlotToIndex(0, 1), ConvertSlotToIndex(1, 0) }
                    );
                case 1:
                    return (
                        new int[] { Slots[0, 0], Slots[1, 1], Slots[0, 2] },
                        new int[]
                        {
                            ConvertSlotToIndex(0, 0),
                            ConvertSlotToIndex(1, 1),
                            ConvertSlotToIndex(0, 2)
                        }
                    );
                case 2:
                    return (
                        new int[] { Slots[0, 1], Slots[1, 2] },
                        new int[] { ConvertSlotToIndex(0, 1), ConvertSlotToIndex(1, 2) }
                    );
                case 3:
                    return (
                        new int[] { Slots[0, 0], Slots[1, 1], Slots[2, 1] },
                        new int[]
                        {
                            ConvertSlotToIndex(0, 0),
                            ConvertSlotToIndex(1, 1),
                            ConvertSlotToIndex(2, 1)
                        }
                    );
                case 7:
                    return (
                        new int[]
                        {
                            Slots[1, 0],
                            Slots[1, 1],
                            Slots[1, 2],
                            Slots[3, 0],
                            Slots[3, 1],
                            Slots[3, 2]
                        },
                        new int[]
                        {
                            ConvertSlotToIndex(1, 0),
                            ConvertSlotToIndex(1, 1),
                            ConvertSlotToIndex(1, 2),
                            ConvertSlotToIndex(3, 0),
                            ConvertSlotToIndex(3, 1),
                            ConvertSlotToIndex(3, 2)
                        }
                    );
                case 9:
                    return (
                        new int[] { Slots[2, 1], Slots[4, 1], Slots[4, 0] },
                        new int[]
                        {
                            ConvertSlotToIndex(2, 1),
                            ConvertSlotToIndex(4, 1),
                            ConvertSlotToIndex(4, 0)
                        }
                    );
                case 10:
                    return (
                        new int[] { Slots[3, 0], Slots[2, 1], Slots[3, 2], Slots[4, 1] },
                        new int[]
                        {
                            ConvertSlotToIndex(3, 0),
                            ConvertSlotToIndex(2, 1),
                            ConvertSlotToIndex(3, 2),
                            ConvertSlotToIndex(4, 1)
                        }
                    );
                case 11:
                    return (
                        new int[] { Slots[3, 1], Slots[2, 1], Slots[4, 2] },
                        new int[]
                        {
                            ConvertSlotToIndex(3, 1),
                            ConvertSlotToIndex(2, 1),
                            ConvertSlotToIndex(4, 2)
                        }
                    );
                case 12:
                    return (
                        new int[] { Slots[3, 0], Slots[4, 1] },
                        new int[] { ConvertSlotToIndex(3, 0), ConvertSlotToIndex(4, 1) }
                    );
                case 13:
                    return (
                        new int[] { Slots[4, 0], Slots[3, 1], Slots[4, 2] },
                        new int[]
                        {
                            ConvertSlotToIndex(4, 0),
                            ConvertSlotToIndex(3, 1),
                            ConvertSlotToIndex(4, 2)
                        }
                    );
                case 14:
                    return (
                        new int[] { Slots[4, 1], Slots[3, 2] },
                        new int[] { ConvertSlotToIndex(4, 1), ConvertSlotToIndex(3, 2) }
                    );
                default:
                    Console.WriteLine("ERROR 926491 - Invalid slot index");
                    return (new int[] { }, new int[] { });
            }
        }

        private int GetSlot(int indexSlot)
        { // 0 - 14
            if (indexSlot < 0 || indexSlot > 14)
            {
                Console.WriteLine("ERROR 208416 - Invalid slot index (ID: " + indexSlot + ")");
                return -2; // invalid slot index
            }
            else
                return Slots[
                    ConvertIndexToSlot(indexSlot).Item1,
                    ConvertIndexToSlot(indexSlot).Item2
                ];
        }

        public int ConvertSlotToIndex(int slot1, int slot2) => slot1 * 3 + slot2; // index ( 0 - 14)

        public (int, int) ConvertIndexToSlot(int indexSlot) =>
            ((int)indexSlot / 3, (int)indexSlot % 3); // slot ( (0, 0) )
    }
}
