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
                { 1, 1, 1 },
                {-1, 0,-1 },
                { 2, 2, 2 },
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

            int[] firstSpace = { 1, 2, 3, 9, 13, 14 };
            int[] secondSpace = { 5, 11 };

            foreach (var slot in Slots)
            {

                foreach (int i in firstSpace)
                {
                    if (i == slot) map += " ";
                }

                switch (slot)
                {
                    case 1:
                        map += "⊚";
                        break;
                    case 2:
                        map += "⦿";
                        break;
                    default:
                        break;
                }

                foreach (int i in secondSpace)
                {
                    if (i == slot) map += " ";
                }

                index++;

            }
            return map;
        }
    }
}