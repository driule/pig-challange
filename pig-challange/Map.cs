using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Map
    {
        Random rand = new Random();

        private Agent agentA, agentB;
        private Pig pig;
        private int[,] grid;
        private int iteration;
        
        public Map()
        {
            this.agentA = new Agent(this.GetRandomStartPosition()); 
            this.agentB = new Agent(this.GetRandomStartPosition());
            this.pig = new Pig(this.GetRandomStartPosition());

            this.grid = new int[9, 9];

        }

        public void RunGame()
        {

            for (int i = 0; i < 15; i++)
            {
                Tuple<int, int> newPositionAgentA = this.agentA.DetermineStep();
                Tuple<int, int> newPositionAgentB = this.agentB.DetermineStep();
                Tuple<int, int> newPositionPig = this.pig.DetermineStep();

                this.Draw();
            }

        }

        public void Draw()
        {

        }

        private Tuple<int,int> GetRandomStartPosition()
        {
            int y = rand.Next(2, 7);
            int x = rand.Next(2, 7);

            return new Tuple<int, int>(y, x);
            // TODO: check if positions don't collide

        }
    }
}
