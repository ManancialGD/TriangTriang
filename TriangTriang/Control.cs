using TriangTriang.Models;

namespace TriangTriang.Controls
{
    class Control
    {
        Map map;
        public Control()
        {
            
        }

        public void Initialize()
        {
            map = new Map();
        }

        public Map GetMap()
        {
            return map;
        }
    }
}