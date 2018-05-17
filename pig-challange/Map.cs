using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Map
    {
        private Agent agentA, agentB;
        private Pig pig;
        private int[,] grid;
        private int iteration;
        
        public Map()
        {
            agentA = new Agent();
            agentB = new Agent();
            pig = new Pig();
            grid = new int[9, 9];

        }

        public void RunGame()
        {

        }

        public void Draw()
        {

        }
    }
}
