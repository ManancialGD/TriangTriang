namespace TriangTriang.Models
{
    class Map
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
    }
}