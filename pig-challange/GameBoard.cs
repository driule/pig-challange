using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class GameBoard
    {
        Random randomizer = new Random();

        private Agent agentA, agentB;
        private Pig pig;
        private Map map;
        
        public GameBoard()
        {
            this.agentA = new Agent(this.GetRandomStartPosition()); 
            this.agentB = new Agent(this.GetRandomStartPosition());
            this.pig = new Pig(this.GetRandomStartPosition());

            this.map = new Map();
        }

        public void RunGame()
        {
            for (int i = 0; i < 15; i++)
            {
                this.agentA.DetermineStep(this.map);
                this.agentB.DetermineStep(this.map);
                this.pig.DetermineStep(this.map, this.agentA, this.agentB);

                this.map.Draw(this.agentA, this.agentB, this.pig);

                this.EvaluateGameState();
            }
        }

        // TODO: implement game end (if pig was caught or agent exited) and score tracking
        private void EvaluateGameState()
        {
        }

        // TODO: check if positions of agents and pig don't collide
        private Tuple<int,int> GetRandomStartPosition()
        {
            int x = 0, y = 0;
            while (true)
            {
                x = this.randomizer.Next(2, 7);
                y = this.randomizer.Next(2, 7);

                // check for obstacles
                if (x == 3 && y == 3 || x == 3 && y == 5 || x == 5 && y == 3 || x == 5 && y == 5)
                {
                    continue;
                }

                break;
            }

            return new Tuple<int, int>(y, x);
        }
    }
}
